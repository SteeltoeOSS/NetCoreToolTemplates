using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class LoggingDynamicSerilogOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("logging-dynamic-serilog", "Enable dynamically changing minimum levels at runtime using Serlog.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
    }

    private static string GetPackageName(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Logging.DynamicSerilogCore" : "Steeltoe.Logging.DynamicSerilog";
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)}");
        snippets.Add(GetSetupCodeFragment(options.SteeltoeVersion));
    }

    private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Logging.DynamicSerilog" : "Steeltoe.Logging.DynamicSerilog";
    }

    private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "builder.AddDynamicSerilog();" : "builder.Logging.AddDynamicSerilog();";
    }
}
