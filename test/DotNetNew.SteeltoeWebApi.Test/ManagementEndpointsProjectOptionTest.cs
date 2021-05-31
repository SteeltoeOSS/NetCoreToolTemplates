using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
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
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add(("Steeltoe.Management.CloudFoundryCore", "$(SteeltoeVersion)"));
                    break;
                default:
                    packages.Add(("Steeltoe.Management.EndpointCore", "$(SteeltoeVersion)"));
                    break;
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.Management.CloudFoundry;");
                    snippets.Add("services.AddCloudFoundryActuators(Configuration);");
                    snippets.Add("app.UseCloudFoundryActuators()");
                    break;
                default:
                    snippets.Add("using Steeltoe.Management.Endpoint;");
                    snippets.Add("services.AddAllActuators(Configuration);");
                    break;
            }
        }
    }
}
