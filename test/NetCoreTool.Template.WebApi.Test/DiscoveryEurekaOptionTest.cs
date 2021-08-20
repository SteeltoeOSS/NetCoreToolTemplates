using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DiscoveryEurekaOptionTest : ProjectOptionTest
    {
        public DiscoveryEurekaOptionTest(ITestOutputHelper logger) : base("discovery-eureka",
            "Add access to Eureka, a REST-based service for locating services", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                packages.Add(("Steeltoe.Discovery.ClientCore", "$(SteeltoeVersion)"));
            }

            if (steeltoeVersion >= SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.Discovery.Eureka", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("services.AddDiscoveryClient(Configuration)");
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                snippets.Add("app.UseDiscoveryClient()");
            }
        }
    }
}
