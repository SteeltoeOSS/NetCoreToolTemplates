using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationRandomValueOptionTest : ProjectOptionTest
    {
        public ConfigurationRandomValueOptionTest(ITestOutputHelper logger) : base("configuration-random-value",
            "Add a random value configuration source", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Configuration.RandomValueBase", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Extensions.Configuration.RandomValue");
            snippets.Add(".ConfigureAppConfiguration");
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async void TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Controllers/RandomValueController.cs").Should().BeFalse();
            sandbox.FileExists("Controllers/RandomValueController.fs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Controllers/RandomValueController");
            Sandbox.FileExists(GetSourceFileForLanguage("Controllers/RandomValueController", options.Language)).Should().BeTrue();
        }
    }
}
