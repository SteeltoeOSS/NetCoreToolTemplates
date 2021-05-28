using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class ManagementEndpointsOptionTest : OptionTest
    {
        public ManagementEndpointsOptionTest(ITestOutputHelper logger) : base("management-endpoints",
            "Add application management endpoints, such as health and metrics", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> packages)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add("Steeltoe.Management.CloudFoundryCore");
                    break;
                default:
                    packages.Add("Steeltoe.Management.EndpointCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
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
