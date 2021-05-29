using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Steeltoe.DotNetNew.Test.Utilities;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public abstract class OptionTest : TemplateTest
    {
        protected OptionTest(string option, string help, ITestOutputHelper logger) : base(logger)
        {
            _option = option;
            _help = help;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
            if (_option is not null)
            {
                Logger.WriteLine($"option: {_option}");
            }
        }

        private readonly string _option;

        private readonly string _help;

        protected Sandbox Sandbox { get; private set; }

        protected bool SkipProjectGeneration { get; init; } = false;

        protected string SmokeTestOption { get; init; } = string.Empty;

        [Fact]
        [Trait("Category", "Smoke")]
        public async void SmokeTest()
        {
            (await TemplateSandbox(SmokeTestOption)).Dispose();
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestHelp()
        {
            Logger.WriteLine($"testing help");
            using var sandbox = await TemplateSandbox("--help");
            var period = _option is null ? "" : ".";
            sandbox.CommandOutput.Should().ContainSnippet($"{_option} {_help}{period}");
        }

        [Theory]
        [Trait("Category", "Functional")]
        [ClassData(typeof(TemplateOptions.SteeltoeVersionsAndFrameworks))]
        public async void TestProjectGeneration(string steeltoeOption, string frameworkOption)
        {
            Logger.WriteLine($"steeltoe/framework: {steeltoeOption}/{frameworkOption}");
            if (SkipProjectGeneration)
            {
                Logger.WriteLine("skipping project generation");
                return;
            }

            var steeltoe = ToSteeltoeEnum(steeltoeOption);
            var framework = ToFrameworkEnum(frameworkOption);
            Sandbox = await TemplateSandbox($"--steeltoe {steeltoeOption} --framework {frameworkOption}");
            try
            {
                await AssertProject(steeltoe, framework);
            }
            finally
            {
                Sandbox.Dispose();
                Sandbox = null;
            }
        }

        protected virtual async Task AssertProject(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            Logger.WriteLine("asserting project");
            await AssertCsproj(steeltoeVersion, framework);
            await AssertProgramCs(steeltoeVersion, framework);
            await AssertStartupCs(steeltoeVersion, framework);
            await AssertValuesControllerCs(steeltoeVersion, framework);
            await AssertAppSettingsJson(steeltoeVersion, framework);
            await AssertLaunchSettingsJson(steeltoeVersion, framework);
        }

        private async Task AssertCsproj(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            Logger.WriteLine("asserting .csproj");
            var project = await Sandbox.GetXmlDocumentAsync($"{Sandbox.Name}.csproj");

            var expectedPackages = new List<(string, string)>();
            AddProjectPackages(steeltoeVersion, framework, expectedPackages);
            if (expectedPackages.Count == 0)
            {
                Logger.WriteLine("no packages to assert");
            }
            else
            {
                Logger.WriteLine("asserting packages");
                var packages =
                (
                    from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                    select (e.Attribute("Include")?.Value, e.Attribute("Version")?.Value)
                ).ToList();
                packages.Should().Contain(expectedPackages);
            }

            var expectedProperties = new Dictionary<string, string>();
            AddProjectProperties(steeltoeVersion, framework, expectedProperties);
            if (expectedProperties.Count == 0)
            {
                Logger.WriteLine("no properties to assert");
            }
            else
            {
                Logger.WriteLine("asserting properties");
                var properties =
                (
                    from e in project.Elements().Elements("PropertyGroup").Elements()
                    select e
                ).ToArray().ToDictionary(e => e.Name.ToString(), e => e.Value);
                properties.Should().Contain(expectedProperties);
            }
        }

        protected virtual void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
        }

        protected virtual void AddProjectProperties(SteeltoeVersion steeltoeVersion, Framework framework,
            Dictionary<string, string> properties)
        {
        }

        private async Task AssertProgramCs(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var snippets = new List<string>();
            AddProgramCsSnippets(steeltoeVersion, framework, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Program.cs source snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Program.cs");
            var source = await Sandbox.GetFileTextAsync("Program.cs");
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AddProgramCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
        }

        private async Task AssertStartupCs(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var snippets = new List<string>();
            AddStartupCsSnippets(steeltoeVersion, framework, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Startup.cs source snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Startup.cs");
            var source = await Sandbox.GetFileTextAsync("Startup.cs");
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
        }

        private async Task AssertValuesControllerCs(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var snippets = new List<string>();
            AddValuesControllerCsSnippets(steeltoeVersion, framework, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no ValuesController.cs source snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Controllers/ValuesController.cs");
            var source = await Sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AddValuesControllerCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
        }

        private async Task AssertAppSettingsJson(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var assertions = new List<Action<SteeltoeVersion, Framework, AppSettings>>();
            AddAppSettingsAssertions(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no appsettings.json assertions to assert");
                return;
            }

            Logger.WriteLine("asserting appsettings.json");
            var settings = await Sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json");
            foreach (var assertion in assertions)
            {
                assertion(steeltoeVersion, framework, settings);
            }
        }

        protected virtual void AddAppSettingsAssertions(
            List<Action<SteeltoeVersion, Framework, AppSettings>> assertions)
        {
        }

        private async Task AssertLaunchSettingsJson(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var assertions = new List<Action<SteeltoeVersion, Framework, LaunchSettings>>();
            AddLaunchSettingsAssertions(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no Properties/launchSettings.json assertions to assert");
                return;
            }

            Logger.WriteLine("asserting Properties/launchSettings.json");
            var settings = await Sandbox.GetJsonDocumentAsync<LaunchSettings>("properties/launchSettings.json");
            foreach (var assertion in assertions)
            {
                assertion(steeltoeVersion, framework, settings);
            }
        }

        protected virtual void AddLaunchSettingsAssertions(
            List<Action<SteeltoeVersion, Framework, LaunchSettings>> assertions)
        {
        }

        protected override async Task<Sandbox> TemplateSandbox(string args = "")
        {
            var normalizedArgs = new StringBuilder();

            if (!args.Contains("--help"))
            {
                if (!args.Contains("--no-restore"))
                {
                    normalizedArgs.Append(" --no-restore");
                }

                if (_option is not null)
                {
                    normalizedArgs.Append(" --").Append(_option);
                }
            }

            normalizedArgs.Append($" {args}");

            return await base.TemplateSandbox(normalizedArgs.ToString().Trim());
        }

        private static SteeltoeVersion ToSteeltoeEnum(string steeltoe)
        {
            if (steeltoe.StartsWith("3."))
            {
                return SteeltoeVersion.Steeltoe3;
            }

            if (steeltoe.StartsWith("2."))
            {
                return SteeltoeVersion.Steeltoe2;
            }

            throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe);
        }

        private static Framework ToFrameworkEnum(string framework)
        {
            return framework switch
            {
                "net5.0" => Framework.Net50,
                "netcoreapp3.1" => Framework.NetCoreApp31,
                "netcoreapp2.1" => Framework.NetCoreApp21,
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework)
            };
        }
    }
}
