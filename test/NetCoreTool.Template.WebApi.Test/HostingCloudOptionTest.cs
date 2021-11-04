using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class HostingCloudOptionTest : ProjectOptionTest

    {
        public HostingCloudOptionTest(ITestOutputHelper logger) : base("hosting-cloud",
            "Add hosting support for clouds", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Common.Hosting");
            snippets.Add(".UseCloudHosting(");
        }
    }
}
