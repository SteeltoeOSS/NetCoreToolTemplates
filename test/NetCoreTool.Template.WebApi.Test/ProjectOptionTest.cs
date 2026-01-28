using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ProjectOptionTest(string option, string description, ITestOutputHelper logger)
        : OptionTest(option, description, logger)
    {
        private static readonly Regex FrameworkRegex = new(@"^net(?<number>[0-9]+)\.0$", RegexOptions.Compiled);

        [Theory]
        [Trait("Category", "ProjectGeneration")]
        [ClassData(typeof(TemplateOptions.BaseOptions))]
        public virtual async Task Project_Should_Be_Generated(string steeltoeOption, string frameworkOption,
            string languageOption)
        {
            Logger.WriteLine($"steeltoe/framework/language/option: {steeltoeOption}/{frameworkOption}/{languageOption}/{Option}");
            using var _ = Sandbox = await TemplateSandbox($"--steeltoe {steeltoeOption} --framework {frameworkOption} --language {languageOption} --no-restore");
            var options = ToProjectOptions(steeltoeOption, frameworkOption, languageOption);
            await AssertProjectGeneration(options);
        }

        [Theory]
        [Trait("Category", "ProjectBuild")]
        [ClassData(typeof(TemplateOptions.BaseOptions))]
        public virtual async Task Project_Should_Be_Built(string steeltoeOption, string frameworkOption, string languageOption)
        {
            Logger.WriteLine($"steeltoe/framework/language/option: {steeltoeOption}/{frameworkOption}/{languageOption}/{Option}");
            Logger.WriteLine("generating project");

            using var sandbox = await TemplateSandbox($"--steeltoe {steeltoeOption} --framework {frameworkOption} --language {languageOption}");
            sandbox.CommandExitCode.Should().Be(0, $"generation should succeed, while output was:{Environment.NewLine}{sandbox.CommandOutput}");

            Logger.WriteLine("building project");
            await CreateEditorConfigForUnnecessaryUsings(sandbox);
            var buildCmd = "dotnet build /p:TreatWarningsAsErrors=True /p:EnforceCodeStyleInBuild=True /p:GenerateDocumentationFile=True";
            await sandbox.ExecuteCommandAsync(buildCmd, false);
            sandbox.CommandExitCode.Should().Be(0, $"build should succeed, while output was:{Environment.NewLine}{sandbox.CommandOutput}");
        }

        private static async Task CreateEditorConfigForUnnecessaryUsings(Sandbox sandbox)
        {
            await File.WriteAllTextAsync(Path.Combine(sandbox.Path, ".editorconfig"),
                """
                root = true
                [*]
                # Remove unnecessary using directives
                dotnet_diagnostic.IDE0005.severity = warning
                # Missing XML comment for publicly visible type or member
                dotnet_diagnostic.CS1591.severity = none
                """);
        }

        protected virtual async Task AssertProjectGeneration(ProjectOptions options)
        {
            Logger.WriteLine("asserting project generation");
            await AssertPackageReferences(options);
            await AssertProjectProperties(options);
            await AssertProgramSnippets(options);
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

            var packagesWithUnevaluatedCondition =
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                where e.Attribute("Condition") != null
                select (e.Attribute("Include")?.Value, e.Attribute("Condition")?.Value);
            packagesWithUnevaluatedCondition.Should().BeEmpty();

            var packages =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                select (InQuotes(e.Attribute("Include")?.Value), InQuotes(e.Attribute("Version")?.Value))
            ).ToList();
            packages.Should().Contain(expectedPackages.Select((p) => (InQuotes(p.Item1), InQuotes(p.Item2))));
        }

        protected static string InQuotes(string source)
        {
            return source == null ? null : '"' + source + '"';
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

        private async Task AssertAppSettingsJson(ProjectOptions options)
        {
            var assertions = new List<Action<ProjectOptions, AppSettings>>();

            AssertAppSettingsJsonHook(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no appsettings.json assertions");
            }
            else
            {
                Logger.WriteLine("asserting appsettings.json");
                var settings = await Sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json");
                foreach (var assertion in assertions)
                {
                    assertion(options, settings);
                }
            }

            assertions.Clear();
            AssertDevelopmentAppSettingsJsonHook(assertions);
            if (assertions.Count == 0)
            {
                Logger.WriteLine("no appsettings.Development.json assertions");
            }
            else
            {
                Logger.WriteLine("asserting appsettings.Development.json");
                var settings = await Sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.Development.json");
                foreach (var assertion in assertions)
                {
                    assertion(options, settings);
                }
            }
        }

        protected virtual void AssertAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
        }

        protected virtual void AssertDevelopmentAppSettingsJsonHook(
            List<Action<ProjectOptions, AppSettings>> assertions)
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
            var settings = await Sandbox.GetJsonDocumentAsync<LaunchSettings>($"Properties{Path.DirectorySeparatorChar}launchSettings.json");
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
                _ => throw new ArgumentOutOfRangeException(nameof(language), language.ToString())
            };
            return $"{baseName}{ext}";
        }

        protected static string GetSourceFileForLanguage(string baseName, Language language)
        {
            var ext = language switch
            {
                Language.CSharp => ".cs",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language.ToString())
            };
            return $"{baseName}{ext}";
        }

        protected static string GetPackageVersionForFramework(Framework framework)
        {
            return $"{(int)framework / 10}.0.*";
        }

        private static ProjectOptions ToProjectOptions(string steeltoeVersion, string framework, string language)
        {
            return new ProjectOptions
            {
                SteeltoeVersion = ToSteeltoeEnum(steeltoeVersion),
                IsUnstableVersion = steeltoeVersion.Contains("-main"),
                Framework = ToFrameworkEnum(framework),
                Language = ToLanguageEnum(language)
            };
        }

        private static SteeltoeVersion ToSteeltoeEnum(string steeltoe)
        {
            if (steeltoe.StartsWith("3.2"))
            {
                return SteeltoeVersion.Steeltoe32;
            }
            if (steeltoe.StartsWith("4.0"))
            {
                return SteeltoeVersion.Steeltoe40;
            }
            if (steeltoe.StartsWith("4.*"))
            {
                return SteeltoeVersion.SteeltoeUnstable;
            }

            throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe);
        }

        private static Framework ToFrameworkEnum(string framework)
        {
            var match = FrameworkRegex.Match(framework);

            if (match.Success)
            {
                var capture = match.Groups["number"].Value;
                if (int.TryParse(capture, out var number))
                {
                    var majorVersion = number * 10;
                    if (Enum.IsDefined(typeof(Framework), majorVersion))
                    {
                        return (Framework)majorVersion;
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(framework), framework);
        }

        private static Language ToLanguageEnum(string language)
        {
            return language switch
            {
                "C#" => Language.CSharp,
                _ => throw new ArgumentOutOfRangeException(nameof(language), language)
            };
        }
    }
}
