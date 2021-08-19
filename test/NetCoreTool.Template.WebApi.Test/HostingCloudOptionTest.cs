using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudOptionTest : ProjectOptionTest

    {
        public HostingCloudOptionTest(ITestOutputHelper logger) : base("hosting-cloud",
            "Add hosting support for clouds", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Common.Hosting;");
            snippets.Add(".UseCloudHosting()");
        }
    }
}
