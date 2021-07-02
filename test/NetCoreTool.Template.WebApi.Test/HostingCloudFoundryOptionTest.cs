using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudFoundryOptionTest : ProjectOptionTest

    {
        public HostingCloudFoundryOptionTest(ITestOutputHelper logger) : base("hosting-cloud-foundry",
            "Add hosting support for Cloud Foundry", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.Extensions.Configuration.CloudFoundryCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Common.Hosting;");
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add("services.ConfigureCloudFoundryOptions(Configuration);");
        }
    }
}
