using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class ManagementEndpointsOptionTest : OptionTest
    {
        public ManagementEndpointsOptionTest(ITestOutputHelper logger) : base("management-endpoints",
            "Add application management endpoints, such as health and metrics", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packages.Add("Steeltoe.Management.CloudFoundryCore");
                    break;
                default:
                    packages.Add("Steeltoe.Management.EndpointCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
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
