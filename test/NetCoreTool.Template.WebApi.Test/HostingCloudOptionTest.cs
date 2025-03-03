using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("hosting-cloud", "Add support for listening on the port specified by the hosting environment (Steeltoe 3.x only).", logger)

    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                snippets.Add("using Steeltoe.Common.Hosting;");
                snippets.Add("builder.UseCloudHosting();");
            }
        }
    }
}
