using System;
using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationPlaceholderOptionTest : ProjectOptionTest
    {
        public ConfigurationPlaceholderOptionTest(ITestOutputHelper logger) : base("configuration-placeholder",
            "Add a placeholder configuration source", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Configuration.PlaceholderCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Extensions.Configuration.Placeholder");
            snippets.Add(".AddPlaceholderResolver()");
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
