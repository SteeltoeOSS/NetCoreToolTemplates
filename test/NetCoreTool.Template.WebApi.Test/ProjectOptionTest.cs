using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities.Assertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities.Models;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ProjectOptionTest : OptionTest
    {
        protected ProjectOptionTest(string option, string description, ITestOutputHelper logger) : base(option,
            description,
            logger)
        {
        }

        [Theory]
        [Trait("Category", "ProjectGeneration")]
        [ClassData(typeof(TemplateOptions.SteeltoeVersionsAndFrameworks))]
        public async void Project_Should_Be_Generated(string steeltoeOption, string frameworkOption)
        {
            Logger.WriteLine($"steeltoe/framework/option: {steeltoeOption}/{frameworkOption}/{Option}");
            Sandbox = await TemplateSandbox(
                $"--steeltoe {steeltoeOption} --framework {frameworkOption} --no-restore");
            try
            {
                await AssertProjectGeneration(ToSteeltoeEnum(steeltoeOption), ToFrameworkEnum(frameworkOption));
            }
            finally
            {
                Sandbox.Dispose();
                Sandbox = null;
            }
        }

        [Theory]
        [Trait("Category", "ProjectBuild")]
        [ClassData(typeof(TemplateOptions.SteeltoeVersionsAndFrameworks))]
        public async void Project_Should_Be_Built(string steeltoeOption, string frameworkOption)
        {
            Logger.WriteLine($"steeltoe/framework/option: {steeltoeOption}/{frameworkOption}/{Option}");
            Logger.WriteLine("generating project");
            using var sandbox = await TemplateSandbox($"--steeltoe {steeltoeOption} --framework {frameworkOption}");
            Logger.WriteLine("building project");
            await sandbox.ExecuteCommandAsync("dotnet build /p:TreatWarningsAsErrors=True");
        }

        protected virtual async Task AssertProjectGeneration(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            Logger.WriteLine("asserting project generation");
            await AssertCsprojPackages(steeltoeVersion, framework);
            await AssertCsprojProperties(steeltoeVersion, framework);
            await AssertProgramCsSnippets(steeltoeVersion, framework);
            await AssertStartupCsSnippets(steeltoeVersion, framework);
            await AssertAppSettingsJson(steeltoeVersion, framework);
            await AssertLaunchSettingsJson(steeltoeVersion, framework);
        }

        private async Task AssertCsprojPackages(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var expectedPackages = new List<(string, string)>();
            AssertCsprojPackagesHook(steeltoeVersion, framework, expectedPackages);
            if (expectedPackages.Count == 0)
            {
                Logger.WriteLine("no .csproj packages to assert");
                return;
            }

            Logger.WriteLine("asserting .csproj packages");
            var project = await Sandbox.GetXmlDocumentAsync($"{Sandbox.Name}.csproj");
            var packages =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                select (e.Attribute("Include")?.Value, e.Attribute("Version")?.Value)
            ).ToList();
            packages.Should().Contain(expectedPackages);
        }

        protected virtual void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
        }

        private async Task AssertCsprojProperties(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var expectedProperties = new Dictionary<string, string>();
            AssertCsprojPropertiesHook(steeltoeVersion, framework, expectedProperties);
            if (expectedProperties.Count == 0)
            {
                Logger.WriteLine("no .csproj properties to assert");
                return;
            }

            Logger.WriteLine("asserting .csproj properties");
            var project = await Sandbox.GetXmlDocumentAsync($"{Sandbox.Name}.csproj");
            var properties =
            (
                from e in project.Elements().Elements("PropertyGroup").Elements()
                select e
            ).ToArray().ToDictionary(e => e.Name.ToString(), e => e.Value);
            properties.Should().Contain(expectedProperties);
            properties.Keys.Should().NotContain("Description");
        }

        protected virtual void AssertCsprojPropertiesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            Dictionary<string, string> properties)
        {
        }

        private async Task AssertProgramCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var snippets = new List<string>();

            AssertProgramCsSnippetsHook(steeltoeVersion, framework, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Program.cs snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Program.cs snippets");
            var source = await Sandbox.GetFileTextAsync("Program.cs");
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
        }

        private async Task AssertStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var snippets = new List<string>();

            AssertStartupCsSnippetsHook(steeltoeVersion, framework, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Startup.cs snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Startup.cs snippets");
            var source = await Sandbox.GetFileTextAsync("Startup.cs");
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
        }

        private async Task AssertAppSettingsJson(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var assertions = new List<Action<SteeltoeVersion, Framework, AppSettings>>();

            AssertAppSettingsJsonHook(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no appsettings.json assertions");
                return;
            }

            Logger.WriteLine("asserting appsettings.json");
            var settings = await Sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json");
            foreach (var assertion in assertions)
            {
                assertion(steeltoeVersion, framework, settings);
            }
        }

        protected virtual void AssertAppSettingsJsonHook(
            List<Action<SteeltoeVersion, Framework, AppSettings>> assertions)
        {
        }

        private async Task AssertLaunchSettingsJson(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            var assertions = new List<Action<SteeltoeVersion, Framework, LaunchSettings>>();

            AssertLaunchSettingsHook(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no launchSettings.json assertions");
                return;
            }

            Logger.WriteLine("asserting launchSettings.json");
            var settings = await Sandbox.GetJsonDocumentAsync<LaunchSettings>("properties/launchSettings.json");
            foreach (var assertion in assertions)
            {
                assertion(steeltoeVersion, framework, settings);
            }
        }

        protected virtual void AssertLaunchSettingsHook(
            List<Action<SteeltoeVersion, Framework, LaunchSettings>> assertions)
        {
        }

        private static SteeltoeVersion ToSteeltoeEnum(string steeltoe)
        {
            if (steeltoe.StartsWith("3.1."))
            {
                return SteeltoeVersion.Steeltoe31;
            }

            if (steeltoe.StartsWith("3.0."))
            {
                return SteeltoeVersion.Steeltoe30;
            }

            if (steeltoe.StartsWith("2.5."))
            {
                return SteeltoeVersion.Steeltoe25;
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
