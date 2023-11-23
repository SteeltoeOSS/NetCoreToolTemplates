using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingAzureSpringCloudOptionTest : ProjectOptionTest
    {
        public HostingAzureSpringCloudOptionTest(ITestOutputHelper logger) : base("hosting-azure-spring-cloud",
            "Add hosting support for Microsoft Azure Spring Cloud", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            switch (options.SteeltoeVersion)
            {
                default:
                    packages.Add(("Microsoft.Azure.SpringCloud.Client", "2.0.0-preview.3"));
                    break;
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Microsoft.Azure.SpringCloud.Client");
            snippets.Add(".UseAzureSpringCloudService");
        }
    }
}
