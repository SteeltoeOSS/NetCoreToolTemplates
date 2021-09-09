using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DistributedTracingOptionTest : ProjectOptionTest
    {
        public DistributedTracingOptionTest(ITestOutputHelper logger) : base("distributed-tracing",
            "Add distributed tracing support", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Management.TracingCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Management.Tracing;");
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe25:
                    snippets.Add("services.AddDistributedTracing(Configuration);");
                    break;
                case SteeltoeVersion.Steeltoe30:
                    snippets.Add("services.AddDistributedTracing();");
                    break;
                default:
                    snippets.Add("services.AddDistributedTracingAspNetCore();");
                    break;
            }
        }
    }
}
