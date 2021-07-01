using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class EurekaOptionTest : ProjectOptionTest
    {
        public EurekaOptionTest(ITestOutputHelper logger) : base("eureka",
            "Add access to Eureka, a REST-based service for locating services", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Discovery.ClientCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("services.AddDiscoveryClient(Configuration);");
            snippets.Add("app.UseDiscoveryClient();");
        }
    }
}
