using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ManagementEndpointsOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("management-endpoints", "Add management endpoints, providing info about versions, configuration, mapped routes and HTTP traffic, heap/thread dumps, health checks and changing log levels.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Management.EndpointCore" : "Steeltoe.Management.Endpoint";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
            snippets.Add(GetSetupCodeFragment(options.SteeltoeVersion));
        }

        private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Management.Endpoint" : "Steeltoe.Management.Endpoint.Actuators.All";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "builder.AddAllActuators();" : "builder.Services.AddAllActuators();";
        }
    }
}
