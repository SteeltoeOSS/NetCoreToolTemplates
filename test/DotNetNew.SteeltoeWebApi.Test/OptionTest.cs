using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public abstract class OptionTest
    {
        private readonly string _option;

        private readonly string _help;

        protected readonly ITestOutputHelper Logger;

        protected Sandbox Sandbox;

        protected bool SkipProjectGeneration { get; set; } = false;

        protected string SmokeTestOption { get; set; } = string.Empty;

        protected OptionTest(string option, string help, ITestOutputHelper logger)
        {
            _option = option;
            _help = help;
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
            if (_option is not null)
            {
                Logger.WriteLine($"option: {_option}");
            }
        }

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
        [InlineData("3.0.2", "net5.0")]
        [InlineData("3.0.2", "netcoreapp3.1")]
        [InlineData("2.5.3", "netcoreapp3.1")]
        [InlineData("2.5.3", "netcoreapp2.1")]
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

        protected virtual async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            Logger.WriteLine("asserting project");
            AssertCsproj(steeltoe, framework);
            AssertProgramCs(steeltoe, framework);
            AssertStartupCs(steeltoe, framework);
            AssertValuesControllerCs(steeltoe, framework);
            AssertAppSettingsJson(steeltoe, framework);
            AssertLaunchSettingsJson(steeltoe, framework);
        }

        private async void AssertCsproj(Steeltoe steeltoe, Framework framework)
        {
            Logger.WriteLine("asserting .csproj");
            var project = await Sandbox.GetXmlDocumentAsync($"{Sandbox.Name}.csproj");

            var expectedPackages = new List<string>();
            AddProjectPackages(steeltoe, framework, expectedPackages);
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
                        .Attributes("Include")
                    select e.Value
                ).ToList();
                packages.Should().Contain(expectedPackages);
            }

            var expectedProperties = new Dictionary<string, string>();
            AddProjectProperties(steeltoe, framework, expectedProperties);
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

        protected virtual void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
        }

        protected virtual void AddProjectProperties(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties)
        {
        }

        private async void AssertProgramCs(Steeltoe steeltoe, Framework framework)
        {
            var snippets = new List<string>();
            AddProgramCsSnippets(steeltoe, framework, snippets);
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

        protected virtual void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
        }

        private async void AssertStartupCs(Steeltoe steeltoe, Framework framework)
        {
            var snippets = new List<string>();
            AddStartupCsSnippets(steeltoe, framework, snippets);
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

        protected virtual void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
        }

        private async void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework)
        {
            var snippets = new List<string>();
            AddValuesControllerCsSnippets(steeltoe, framework, snippets);
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

        protected virtual void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
            List<string> snippets)
        {
        }

        private async void AssertAppSettingsJson(Steeltoe steeltoe, Framework framework)
        {
            var assertions = new List<Action<Steeltoe, Framework, AppSettings>>();
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
                assertion(steeltoe, framework, settings);
            }
        }

        protected virtual void AddAppSettingsAssertions(List<Action<Steeltoe, Framework, AppSettings>> assertions)
        {
        }

        private async void AssertLaunchSettingsJson(Steeltoe steeltoe, Framework framework)
        {
            var assertions = new List<Action<Steeltoe, Framework, LaunchSettings>>();
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
                assertion(steeltoe, framework, settings);
            }
        }

        protected virtual void AddLaunchSettingsAssertions(List<Action<Steeltoe, Framework, LaunchSettings>> assertions)
        {
        }

        protected async Task<Sandbox> TemplateSandbox(string args = "")
        {
            Assert.NotNull(args);
            var command = new StringBuilder("dotnet new stwebapi");
            if (!args.Contains("--help"))
            {
                if (!args.Contains("--no-restore"))
                {
                    command.Append(" --no-restore");
                }

                if (_option is not null)
                {
                    command.Append(" --").Append(_option);
                }
            }

            if (args.Length > 1)
            {
                command.Append(' ').Append(args);
            }

            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command.ToString());
            return sandbox;
        }

        protected bool IsSteeltoe2(string steeltoe)
        {
            return steeltoe.StartsWith("2.");
        }

        private static Steeltoe ToSteeltoeEnum(string steeltoe)
        {
            if (steeltoe.StartsWith("3."))
            {
                return Steeltoe.Steeltoe3;
            }

            if (steeltoe.StartsWith("2."))
            {
                return Steeltoe.Steeltoe2;
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
