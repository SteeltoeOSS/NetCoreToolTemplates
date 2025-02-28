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
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Management.TracingCore" : "Steeltoe.Management.Tracing";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Management.Tracing;");
            snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
        }

        private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "builder.Services.AddDistributedTracingAspNetCore();";
                yield return "builder.Services.AddDistributedTracing();";
            }
            else
            {
                yield return "builder.Services.AddTracingLogProcessor();";
            }
        }
    }
}
