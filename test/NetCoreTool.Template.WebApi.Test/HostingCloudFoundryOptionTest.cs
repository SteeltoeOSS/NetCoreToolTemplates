using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudFoundryOptionTest : ProjectOptionTest

    {
        public HostingCloudFoundryOptionTest(ITestOutputHelper logger) : base("hosting-cloud-foundry",
            "Add hosting support for Cloud Foundry", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.Extensions.Configuration.CloudFoundryCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Common.Hosting");
            snippets.Add("Steeltoe.Extensions.Configuration.CloudFoundry");
            snippets.Add(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Extensions.Configuration.CloudFoundry");
            snippets.Add("services.ConfigureCloudFoundryOptions");
        }
    }
}
