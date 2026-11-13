using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ManagementEndpointsOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("management-endpoints", "Add management endpoints, providing info about versions, configuration, mapped routes and HTTP traffic, heap/thread dumps, health checks and changing log levels.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Management.Endpoint", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.Management.GitProperties.Build", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Management.Endpoint.Actuators.All;");
            snippets.Add("builder.Services.AddAllActuators();");
        }
    }
}
