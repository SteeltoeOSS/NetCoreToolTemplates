using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingAzureSpringCloudOptionTest : ProjectOptionTest
    {
        public HostingAzureSpringCloudOptionTest(ITestOutputHelper logger) : base("hosting-azure-spring-cloud",
            "Add hosting support for Microsoft Azure Spring Cloud", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (framework < Framework.NetCoreApp31)
            {
                return;
            }

            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe25:
                    packages.Add(("Microsoft.Azure.SpringCloud.Client", "1.0.0-preview.1"));
                    break;
                default:
                    packages.Add(("Microsoft.Azure.SpringCloud.Client", "2.0.0-preview.1"));
                    break;
            }
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (framework < Framework.NetCoreApp31)
            {
                return;
            }

            snippets.Add("using Microsoft.Azure.SpringCloud.Client;");
            snippets.Add(".UseAzureSpringCloudService()");
        }
    }
}
