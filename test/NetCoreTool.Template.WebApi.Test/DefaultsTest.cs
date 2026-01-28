using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
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
            Sandbox.FileExists($"{Sandbox.Name}.http").Should().BeTrue();
            Sandbox.FileExists("Program.cs").Should().BeTrue();

            if (options.IsUnstableVersion)
            {
                Sandbox.FileExists("nuget.config").Should().BeTrue();
            }
            else
            {
                Sandbox.FileExists("nuget.config").Should().BeFalse();
            }
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            var openApiVersion = GetPackageVersionForFramework(options.Framework);

            if (options.Framework == Framework.Net60)
            {
                packages.Add(("Swashbuckle.AspNetCore", "6.5.0"));
            }
            else if (options.Framework == Framework.Net80)
            {
                packages.Add(("Microsoft.AspNetCore.OpenApi", openApiVersion));
                packages.Add(("Swashbuckle.AspNetCore", "6.6.2"));
            }
            else
            {
                packages.Add(("Microsoft.AspNetCore.OpenApi", openApiVersion));
            }
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
                case SteeltoeVersion.Steeltoe32:
                case SteeltoeVersion.Steeltoe40:
                case SteeltoeVersion.Steeltoe41:
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
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                settings.Schema.Should().Be("https://steeltoe.io/schema/v3/schema.json");
            }
            else
            {
                settings.Schema.Should().BeNull();
            }

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
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                settings.Schema.Should().Be("https://steeltoe.io/schema/v3/schema.json");
            } else
            {
                settings.Schema.Should().BeNull();
            }
        }

        protected override void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(ProjectOptions options, LaunchSettings settings)
        {
            foreach (var profile in settings.Profiles.Values)
            {
                if (options.Framework is Framework.Net60 or Framework.Net80)
                {
                    profile.LaunchBrowser.Should().BeTrue();
                    profile.LaunchUrl.Should().Be("swagger");
                }
                else
                {
                    profile.LaunchBrowser.Should().BeFalse();
                    profile.LaunchUrl.Should().BeNull();
                }

                profile.ApplicationUrl.Should().NotContain("5000").And.NotContain("5001");
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.Framework is Framework.Net60 or Framework.Net80)
            {
                snippets.Add("// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle");
                snippets.Add("builder.Services.AddEndpointsApiExplorer();");
                snippets.Add("builder.Services.AddSwaggerGen();");

                snippets.Add("app.UseSwagger();");
                snippets.Add("app.UseSwaggerUI();");
            }
            else
            {
                snippets.Add("// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi");
                snippets.Add("builder.Services.AddOpenApi();");

                snippets.Add("app.MapOpenApi();");
            }

            if (options.Framework == Framework.Net80)
            {
                snippets.Add(@".WithName(""GetWeatherForecast"")");
                snippets.Add(".WithOpenApi();");
            }
            else
            {
                snippets.Add(@".WithName(""GetWeatherForecast"");");
            }

            snippets.Add("var app = builder.Build();");
            snippets.Add("app.UseHttpsRedirection();");
            snippets.Add(@"app.MapGet(""/weatherforecast"", () =>");
            snippets.Add("app.Run();");
            snippets.Add("internal record WeatherForecast(");

            base.AssertProgramSnippetsHook(options, snippets);
        }
    }
}
