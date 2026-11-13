using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DistributedTracingOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("distributed-tracing", "Add distributed tracing support.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Management.Tracing", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Management.Tracing;");
            snippets.Add("builder.Services.AddTracingLogProcessor();");
        }
    }
}
