using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class LoggingDynamicSerilogOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("logging-dynamic-serilog", "Enable dynamically changing minimum levels at runtime using Serilog.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Steeltoe.Logging.DynamicSerilog", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Steeltoe.Logging.DynamicSerilog");
        snippets.Add("builder.Logging.AddDynamicSerilog();");
    }
}
