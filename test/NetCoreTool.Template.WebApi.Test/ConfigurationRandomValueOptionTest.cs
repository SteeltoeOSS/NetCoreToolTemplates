using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationRandomValueOptionTest : ProjectOptionTest
    {
        public ConfigurationRandomValueOptionTest(ITestOutputHelper logger) : base("configuration-random-value",
            "Add a random value configuration source", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Configuration.RandomValueBase", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.RandomValue");
            snippets.Add(".ConfigureAppConfiguration(b => b.AddRandomValueSource())");
        }
    }
}
