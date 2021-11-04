using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ManagementEndpointsOptionTest : ProjectOptionTest
    {
        public ManagementEndpointsOptionTest(ITestOutputHelper logger) : base("management-endpoints",
            "Add application management endpoints, such as health and metrics", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.Management.CloudFoundryCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Management.EndpointCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("Steeltoe.Management.CloudFoundry");
                snippets.Add("services.AddCloudFoundryActuators(");
                snippets.Add("app.UseCloudFoundryActuators(");
            }
            else
            {
                snippets.Add("Steeltoe.Management.Endpoint");
                if (options.Framework >= Framework.NetCoreApp31)
                {
                    snippets.Add("endpoints.MapAllActuators()");
                }
                else
                {
                    snippets.Add("services.AddAllActuators(");
                }
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion >= SteeltoeVersion.Steeltoe30)
            {
                snippets.Add(".AddDynamicConsole(");
            }
        }
    }
}
