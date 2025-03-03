using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class LoggingDynamicConsoleOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("logging-dynamic-console", "Enable dynamically changing minimum levels at runtime using the .NET console logger.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Logging.DynamicLogger" : "Steeltoe.Logging.DynamicConsole";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)}");
            snippets.Add("builder.Logging.AddDynamicConsole();");
        }

        private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Logging" : "Steeltoe.Logging.DynamicConsole";
        }
    }
}
