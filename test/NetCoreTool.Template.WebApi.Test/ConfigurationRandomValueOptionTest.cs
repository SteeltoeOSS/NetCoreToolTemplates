using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
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
    }
}
