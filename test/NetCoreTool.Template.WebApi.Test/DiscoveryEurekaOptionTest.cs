using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DiscoveryEurekaOptionTest : ProjectOptionTest
    {
        public DiscoveryEurekaOptionTest(ITestOutputHelper logger) : base("discovery-eureka",
            "Add access to Eureka, a REST-based service for locating services", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                packages.Add(("Steeltoe.Discovery.ClientCore", "$(SteeltoeVersion)"));
            }

            if (options.SteeltoeVersion >= SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.Discovery.Eureka", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("services.AddDiscoveryClient");
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                snippets.Add("app.UseDiscoveryClient");
            }
        }
    }
}
