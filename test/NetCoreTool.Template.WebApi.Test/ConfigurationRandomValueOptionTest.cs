using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConfigurationRandomValueOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("configuration-random-value", "Add random value generation to configuration.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Configuration.RandomValue", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Configuration.RandomValue;");
            snippets.Add("builder.Configuration.AddRandomValueSource();");
        }
    }
}
