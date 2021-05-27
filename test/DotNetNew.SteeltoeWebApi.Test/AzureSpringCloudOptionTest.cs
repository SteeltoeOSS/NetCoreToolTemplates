using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class AzureSpringCloudOptionTest : OptionTest
    {
        public AzureSpringCloudOptionTest(ITestOutputHelper logger) : base("azure-spring-cloud",
            "Add hosting support for Microsoft Azure Spring Cloud", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    break;
                default:
                    packages.Add("Microsoft.Azure.SpringCloud.Client");
                    break;
            }
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    break;
                default:
                    snippets.Add("using Microsoft.Azure.SpringCloud.Client;");
                    snippets.Add(".UseAzureSpringCloudService()");
                    break;
            }
        }
    }
}
