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
        snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
        snippets.Add(GetSetupCodeFragment(options.SteeltoeVersion));
    }

    private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Discovery.Client" : "Steeltoe.Discovery.Consul";
    }

    private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32
            ? "builder.Services.AddDiscoveryClient(builder.Configuration);"
            : "builder.Services.AddConsulDiscoveryClient();";
    }
}
