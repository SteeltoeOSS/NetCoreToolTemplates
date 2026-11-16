using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class DiscoveryConsulOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("discovery-consul", "Add a service discovery client for HashiCorp Consul.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Steeltoe.Discovery.Consul", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Steeltoe.Discovery.Consul;");
        snippets.Add("builder.Services.AddConsulDiscoveryClient();");
    }
}
