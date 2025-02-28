using System;
using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationPlaceholderOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("configuration-placeholder", "Add placeholder substitution to configuration.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.PlaceholderCore" : "Steeltoe.Configuration.Placeholder";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
            snippets.Add(GetSetupCodeFragment(options.SteeltoeVersion));
        }

        private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.Placeholder" : "Steeltoe.Configuration.Placeholder";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "builder.AddPlaceholderResolver();" : "builder.Configuration.AddPlaceholderResolver();";
        }

        protected override void AssertAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
            assertions.Add(AssertPlaceholderSettings);
        }

        private void AssertPlaceholderSettings(ProjectOptions options, AppSettings settings)
        {
            settings.ResolvedPlaceholderFromEnvVariables.Should().Be("${PATH?NotFound}");
            settings.ResolvedPlaceholderFromJson.Should()
                .Be("${Logging:LogLevel:System?${Logging:LogLevel:Default}}");
            settings.UnresolvedPlaceholder.Should().Be("${SomKeyNotFound?NotFound}");
        }
    }
}
