using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudFoundryOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("hosting-cloud-foundry", "Add hosting support for running on Cloud Foundry.", logger)

    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Steeltoe.Extensions.Configuration.CloudFoundryCore", "$(SteeltoeVersion)"));
                packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
                packages.Add(("Steeltoe.Connector.CloudFoundry", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Configuration.CloudFoundry", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.AddRange(GetNamespaceImports(options.SteeltoeVersion));
            snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
        }

        private static IEnumerable<string> GetNamespaceImports(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "using Steeltoe.Common.Hosting;";
                yield return "using Steeltoe.Extensions.Configuration.CloudFoundry;";
            }
            else
            {
                yield return "using Steeltoe.Configuration.CloudFoundry;";
            }
        }

        private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "builder.UseCloudHosting();";
                yield return "builder.AddCloudFoundryConfiguration();";
                yield return "builder.Services.ConfigureCloudFoundryOptions(builder.Configuration);";
            }
            else
            {
                yield return "builder.AddCloudFoundryConfiguration();";
            }
        }
    }
}
