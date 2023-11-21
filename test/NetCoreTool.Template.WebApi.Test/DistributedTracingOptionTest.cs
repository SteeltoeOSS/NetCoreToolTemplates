using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DistributedTracingOptionTest : ProjectOptionTest
    {
        public DistributedTracingOptionTest(ITestOutputHelper logger) : base("distributed-tracing",
            "Add distributed tracing support", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Management.TracingCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Management.Tracing");
            switch (options.SteeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe30:
                    snippets.Add("services.AddDistributedTracing");
                    break;
                default:
                    snippets.Add("services.AddDistributedTracingAspNetCore");
                    break;
            }
        }
    }
}
