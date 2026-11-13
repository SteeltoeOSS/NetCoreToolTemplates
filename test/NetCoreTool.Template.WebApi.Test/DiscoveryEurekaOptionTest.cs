using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DiscoveryEurekaOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("discovery-eureka", "Add a service discovery client for Netflix Eureka.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Discovery.Eureka", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Discovery.Eureka;");
            snippets.Add("builder.Services.AddEurekaDiscoveryClient();");
        }
    }
}
