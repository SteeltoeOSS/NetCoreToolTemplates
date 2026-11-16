using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DefaultsTest(ITestOutputHelper logger)
        : ProjectOptionTest(null, "Steeltoe ASP.NET Core Web API (C#)", logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Directory.Build.props").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            Sandbox.FileExists(".gitignore").Should().BeTrue();
            Sandbox.FileExists("app.config").Should().BeTrue();
            Sandbox.FileExists($"{Sandbox.Name}.slnx").Should().BeTrue();
            Sandbox.FileExists($"{Sandbox.Name}.csproj").Should().BeTrue();
            Sandbox.FileExists("Program.cs").Should().BeTrue();

            if (options.IsUnstableVersion)
            {
                Sandbox.FileExists("nuget.config").Should().BeTrue();
            }
            else
            {
                Sandbox.FileExists("nuget.config").Should().BeFalse();
            }

            await AssertHttpFile($"{Sandbox.Name}.http", options);
        }

        private async Task AssertHttpFile(string path, ProjectOptions options)
        {
            Sandbox.FileExists(path).Should().BeTrue();

            Logger.WriteLine("asserting http file");
            var httpFile = await Sandbox.GetFileTextAsync(path);

            var settings = await Sandbox.GetJsonDocumentAsync<LaunchSettings>($"Properties{Path.DirectorySeparatorChar}launchSettings.json");
            var port = settings.Profiles.First().Value.ApplicationUrl.Split(':').Last();

            httpFile.Should().Be(options.Framework == Framework.Net100
                ? $$$"""
                     @{{{Sandbox.Name}}}_HostAddress = http://localhost:{{{port}}}

                     GET {{{{{Sandbox.Name}}}_HostAddress}}/weatherforecast/
                     Accept: application/json

                     ###

                     """
                : $$$"""
                     @{{{Sandbox.Name}}}_HostAddress = http://localhost:{{{port}}}

                     GET {{{{{Sandbox.Name}}}_HostAddress}}/openapi/v1.json
                     Accept: application/json

                     ###

                     GET {{{{{Sandbox.Name}}}_HostAddress}}/weatherforecast/
                     Accept: application/json

                     ###

                     """);
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            var openApiVersion = GetPackageVersionForFramework(options.Framework);
            packages.Add(("Microsoft.AspNetCore.OpenApi", openApiVersion));
        }

        protected override void AssertProjectPropertiesHook(ProjectOptions options,
            Dictionary<string, string> properties)
        {
            AssertSteeltoeVersion(options.SteeltoeVersion);

            properties["TargetFramework"] = GetFramework(options.Framework);
        }

        private static string GetFramework(Framework framework)
        {
            return $"net{(int)framework / 10}.0";
        }

        private static void AssertSteeltoeVersion(SteeltoeVersion steeltoeVersion)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe43:
                case SteeltoeVersion.SteeltoeUnstable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(steeltoeVersion), steeltoeVersion.ToString());
            }
        }

        protected override void AssertAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
            assertions.Add(AssertAppSettings);
        }

        private void AssertAppSettings(ProjectOptions options, AppSettings settings)
        {
            settings.Schema.Should().BeNull();
            settings.ResolvedPlaceholderFromEnvVariables.Should().BeNull();
            settings.UnresolvedPlaceholder.Should().BeNull();
            settings.ResolvedPlaceholderFromJson.Should().BeNull();
        }

        protected override void AssertDevelopmentAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
            assertions.Add(AssertDevelopmentAppSettings);
        }

        private void AssertDevelopmentAppSettings(ProjectOptions options, AppSettings settings)
        {
            settings.Schema.Should().BeNull();
        }

        protected override void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(ProjectOptions options, LaunchSettings settings)
        {
            foreach (var profile in settings.Profiles.Values)
            {
                profile.LaunchBrowser.Should().BeFalse();
                profile.LaunchUrl.Should().BeNull();

                profile.ApplicationUrl.Should().NotContain("5000").And.NotContain("5001");
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi");
            snippets.Add("builder.Services.AddOpenApi();");
            snippets.Add("app.MapOpenApi();");
            snippets.Add(@".WithName(""GetWeatherForecast"");");

            snippets.Add("var app = builder.Build();");
            snippets.Add("app.UseHttpsRedirection();");
            snippets.Add(@"app.MapGet(""/weatherforecast"", () =>");
            snippets.Add("app.Run();");
            snippets.Add("internal record WeatherForecast(");

            base.AssertProgramSnippetsHook(options, snippets);
        }
    }
}
