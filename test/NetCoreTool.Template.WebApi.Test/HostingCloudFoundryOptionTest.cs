using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudFoundryOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("hosting-cloud-foundry", "Add hosting support for running on Cloud Foundry.", logger)

    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Configuration.CloudFoundry", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Configuration.CloudFoundry;");
            snippets.Add("builder.AddCloudFoundryConfiguration();");
        }
    }
}
