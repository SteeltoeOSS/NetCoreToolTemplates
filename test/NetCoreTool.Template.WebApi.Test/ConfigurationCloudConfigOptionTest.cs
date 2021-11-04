using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationCloudConfigOptionTest : ProjectOptionTest
    {
        public ConfigurationCloudConfigOptionTest(ITestOutputHelper logger) : base("configuration-cloud-config",
            "Add a Spring Cloud Config configuration source", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Configuration.ConfigServerCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Extensions.Configuration.ConfigServer");
            snippets.Add("AddConfigServer()");
        }
    }
}
