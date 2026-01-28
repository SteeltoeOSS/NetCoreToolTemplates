using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DockerfileOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("dockerfile", "Add a Dockerfile", logger)
    {
        private const string DockerProfileNameInLaunchSettings = "Container (Dockerfile)";

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Dockerfile").Should().BeFalse();
            sandbox.FileExists(".dockerignore").Should().BeFalse();

            await AssertDefaultLaunchSettings(sandbox);
            await AssertDefaultProjectFile(sandbox);
        }

        private static async Task AssertDefaultLaunchSettings(Sandbox sandbox)
        {
            var settings = await sandbox.GetJsonDocumentAsync<LaunchSettings>($"Properties{Path.DirectorySeparatorChar}launchSettings.json");
            var profileNames = settings.Profiles.Select(profile => profile.Key);

            profileNames.Should().NotContain(DockerProfileNameInLaunchSettings);
        }

        private static async Task AssertDefaultProjectFile(Sandbox sandbox)
        {
            var project = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var properties =
            (
                from e in project.Elements().Elements("PropertyGroup").Elements()
                select e
            ).ToArray().ToDictionary(e => e.Name.ToString(), e => e.Value);

            properties.Keys.Should().NotContain("DockerDefaultTargetOS");
            properties.Keys.Should().NotContain("DockerfileContext");

            var packages =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference")
                select (InQuotes(e.Attribute("Include")?.Value), InQuotes(e.Attribute("Version")?.Value))
            ).ToDictionary();

            packages.Keys.Should().NotContain("Microsoft.VisualStudio.Azure.Containers.Tools.Targets");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            Sandbox.FileExists(".dockerignore").Should().BeTrue();

            Logger.WriteLine("asserting Dockerfile");
            var dockerfile = await Sandbox.GetFileTextAsync("Dockerfile");
            var tag = GetImageTag(options.Framework);
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/aspnet:{tag} AS base");
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/sdk:{tag} AS build");
            var projectFile = GetProjectFileForLanguage(Sandbox.Name, options.Language);
            dockerfile.Should().ContainSnippet($"COPY [\"{projectFile}\", \".\"]");
            dockerfile.Should().ContainSnippet($"RUN dotnet build \"./{projectFile}\"");
            dockerfile.Should().ContainSnippet($"RUN dotnet publish \"./{projectFile}\"");
            dockerfile.Should().ContainSnippet($"ENTRYPOINT [\"dotnet\", \"{Sandbox.Name}.dll\"");

            if (options.Framework == Framework.Net60)
            {
                dockerfile.Should().NotContain("USER $APP_UID");
                dockerfile.Should().ContainSnippet("EXPOSE 80");
                dockerfile.Should().ContainSnippet("EXPOSE 443");
            }
            else
            {
                dockerfile.Should().ContainSnippet("USER $APP_UID");
                dockerfile.Should().ContainSnippet("EXPOSE 8080");
                dockerfile.Should().ContainSnippet("EXPOSE 8081");
            }
        }

        private static string GetImageTag(Framework framework)
        {
            return $"{(int)framework / 10}.0";
        }

        protected override void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(ProjectOptions options, LaunchSettings settings)
        {
            var profileNames = settings.Profiles.Select(profile => profile.Key);
            profileNames.Should().Contain(DockerProfileNameInLaunchSettings);

            var dockerProfile = settings.Profiles[DockerProfileNameInLaunchSettings];

            if (options.Framework is Framework.Net60 or Framework.Net80)
            {
                dockerProfile.LaunchBrowser.Should().BeTrue();
                dockerProfile.LaunchUrl.Should().Be("{Scheme}://{ServiceHost}:{ServicePort}/swagger");
            }
            else
            {
                dockerProfile.LaunchBrowser.Should().BeFalse();
                dockerProfile.LaunchUrl.Should().Be("{Scheme}://{ServiceHost}:{ServicePort}");
            }

            if (options.Framework == Framework.Net60)
            {
                dockerProfile.EnvironmentVariables.Should().ContainKey("ASPNETCORE_URLS");
            }
            else
            {
                dockerProfile.EnvironmentVariables.Should().ContainKey("ASPNETCORE_HTTPS_PORTS");
                dockerProfile.EnvironmentVariables.Should().ContainKey("ASPNETCORE_HTTP_PORTS");
            }
        }

        protected override void AssertProjectPropertiesHook(ProjectOptions options, Dictionary<string, string> properties)
        {
            properties["DockerDefaultTargetOS"] = "Linux";
            properties["DockerfileContext"] = ".";
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Microsoft.VisualStudio.Azure.Containers.Tools.Targets", "1.23.0"));
        }
    }
}
