using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
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
        [ClassData(typeof(TemplateOptions.BaseOptions))]
        public async void Project_Should_Be_Generated(string steeltoeOption, string frameworkOption,
            string languageOption)
        {
            Logger.WriteLine(
                $"steeltoe/framework/language/option: {steeltoeOption}/{frameworkOption}/{languageOption}/{Option}");
            Sandbox = await TemplateSandbox(
                $"--steeltoe {steeltoeOption} --framework {frameworkOption} --language {languageOption} --no-restore");
            var options = ToProjectOptions(steeltoeOption, frameworkOption, languageOption);
            try
            {
                await AssertProjectGeneration(options);
            }
            finally
            {
                Sandbox.Dispose();
                Sandbox = null;
            }
        }

        [Theory]
        [Trait("Category", "ProjectBuild")]
        [ClassData(typeof(TemplateOptions.BaseOptions))]
        public async void Project_Should_Be_Built(string steeltoeOption, string frameworkOption, string languageOption)
        {
            Logger.WriteLine(
                $"steeltoe/framework/language/option: {steeltoeOption}/{frameworkOption}/{languageOption}/{Option}");
            Logger.WriteLine("generating project");
            using var sandbox =
                await TemplateSandbox(
                    $"--steeltoe {steeltoeOption} --framework {frameworkOption} --language {languageOption}");
            sandbox.CommandExitCode.Should().Be(0, sandbox.CommandOutput);
            Logger.WriteLine("building project");
            var buildCmd = "dotnet build";
            // TODO: can we ignore specific F# warnings so we can error if there are unexpected warnings?
            if (ToLanguageEnum(languageOption) == Language.CSharp)
            {
                buildCmd += " /p:TreatWarningsAsErrors=True";
            }

            await sandbox.ExecuteCommandAsync(buildCmd);

            sandbox.CommandExitCode.Should().Be(0, sandbox.CommandOutput);
        }

        protected virtual async Task AssertProjectGeneration(ProjectOptions options)
        {
            Logger.WriteLine("asserting project generation");
            await AssertPackageReferences(options);
            await AssertProjectProperties(options);
            await AssertProgramSnippets(options);
            await AssertStartupSnippets(options);
            await AssertAppSettingsJson(options);
            await AssertLaunchSettingsJson(options);
        }

        private async Task AssertPackageReferences(ProjectOptions options)
        {
            var expectedPackages = new List<(string, string)>();
            AssertPackageReferencesHook(options, expectedPackages);
            if (expectedPackages.Count == 0)
            {
                Logger.WriteLine("no project package references to assert");
                return;
            }

            Logger.WriteLine("asserting project package references");
            var path = GetProjectFileForLanguage(Sandbox.Name, options.Language);
            var project = await Sandbox.GetXmlDocumentAsync(path);
            var packages =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                select (e.Attribute("Include")?.Value, e.Attribute("Version")?.Value)
            ).ToList();
            packages.Should().Contain(expectedPackages);
        }

        protected virtual void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
        }

        private async Task AssertProjectProperties(ProjectOptions options)
        {
            var expectedProperties = new Dictionary<string, string>();
            AssertProjectPropertiesHook(options, expectedProperties);
            if (expectedProperties.Count == 0)
            {
                Logger.WriteLine("no project properties to assert");
                return;
            }

            Logger.WriteLine("asserting project properties");
            var projectFile = GetProjectFileForLanguage(Sandbox.Name, options.Language);
            var project = await Sandbox.GetXmlDocumentAsync(projectFile);
            var properties =
            (
                from e in project.Elements().Elements("PropertyGroup").Elements()
                select e
            ).ToArray().ToDictionary(e => e.Name.ToString(), e => e.Value);
            properties.Should().Contain(expectedProperties);
            properties.Keys.Should().NotContain("Description");
        }

        protected virtual void AssertProjectPropertiesHook(ProjectOptions options,
            Dictionary<string, string> properties)
        {
        }

        private async Task AssertProgramSnippets(ProjectOptions options)
        {
            var snippets = new List<string>();

            AssertProgramSnippetsHook(options, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Program snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Program snippets");
            var path = GetSourceFileForLanguage("Program", options.Language);
            var source = await Sandbox.GetFileTextAsync(path);
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
        }

        private async Task AssertStartupSnippets(ProjectOptions options)
        {
            var snippets = new List<string>();

            AssertStartupSnippetsHook(options, snippets);
            if (snippets.Count == 0)
            {
                Logger.WriteLine("no Startup snippets to assert");
                return;
            }

            Logger.WriteLine("asserting Startup snippets");
            var path = GetSourceFileForLanguage("Startup", options.Language);
            var source = await Sandbox.GetFileTextAsync(path);
            foreach (var snippet in snippets)
            {
                source.Should().ContainSnippet(snippet);
            }
        }

        protected virtual void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
        }

        private async Task AssertAppSettingsJson(ProjectOptions options)
        {
            var assertions = new List<Action<ProjectOptions, AppSettings>>();

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
                assertion(options, settings);
            }
        }

        protected virtual void AssertAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
        }

        private async Task AssertLaunchSettingsJson(ProjectOptions options)
        {
            var assertions = new List<Action<ProjectOptions, LaunchSettings>>();

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
                assertion(options, settings);
            }
        }

        protected virtual void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
        }

        protected static string GetProjectFileForLanguage(string baseName, Language language)
        {
            var ext = language switch
            {
                Language.CSharp => ".csproj",
                Language.FSharp => ".fsproj",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language.ToString())
            };
            return $"{baseName}{ext}";
        }

        protected static string GetSourceFileForLanguage(string baseName, Language language)
        {
            var ext = language switch
            {
                Language.CSharp => ".cs",
                Language.FSharp => ".fs",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language.ToString())
            };
            return $"{baseName}{ext}";
        }

        private static ProjectOptions ToProjectOptions(string steeltoeVerison, string framework, string language)
        {
            return new ProjectOptions
            {
                SteeltoeVersion = ToSteeltoeEnum(steeltoeVerison),
                Framework = ToFrameworkEnum(framework),
                Language = ToLanguageEnum(language),
            };
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
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework)
            };
        }

        private static Language ToLanguageEnum(string language)
        {
            return language switch
            {
                "C#" => Language.CSharp,
                "F#" => Language.FSharp,
                _ => throw new ArgumentOutOfRangeException(nameof(language), language)
            };
        }
    }
}
