using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingAzureSpringCloudOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("hosting-azure-spring-cloud", "Add hosting support for running on Microsoft Azure Spring Cloud (Steeltoe 3.x only).", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Microsoft.Azure.SpringCloud.Client", "2.0.0-preview.3"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                snippets.Add("using Microsoft.Azure.SpringCloud.Client;");
                snippets.Add("builder.WebHost.UseAzureSpringCloudService();");
            }
        }
    }
}
