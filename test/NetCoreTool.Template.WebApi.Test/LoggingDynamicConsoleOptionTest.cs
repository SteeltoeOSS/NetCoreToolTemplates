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
            packages.Add(("Steeltoe.Logging.DynamicConsole", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Logging.DynamicConsole");
            snippets.Add("builder.Logging.AddDynamicConsole();");
        }
    }
}
