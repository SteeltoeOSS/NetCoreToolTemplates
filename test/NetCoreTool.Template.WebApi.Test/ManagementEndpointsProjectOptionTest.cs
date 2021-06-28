using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ManagementEndpointsProjectOptionTest : ProjectOptionTest
    {
        public ManagementEndpointsProjectOptionTest(ITestOutputHelper logger) : base("management-endpoints",
            "Add application management endpoints, such as health and metrics", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.Management.CloudFoundryCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Management.EndpointCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.Management.CloudFoundry;");
                snippets.Add("services.AddCloudFoundryActuators(Configuration);");
                snippets.Add("app.UseCloudFoundryActuators()");
            }
            else
            {
                snippets.Add("using Steeltoe.Management.Endpoint;");
                snippets.Add("services.AddAllActuators(Configuration);");
            }
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion >= SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("ConfigureLogging((context, builder) => builder.AddDynamicConsole())");
            }
        }
    }
}
