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
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.RandomValueBase" : "Steeltoe.Configuration.RandomValue";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
            snippets.Add("builder.Configuration.AddRandomValueSource();");
        }

        private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.RandomValue" : "Steeltoe.Configuration.RandomValue";
        }
    }
}
