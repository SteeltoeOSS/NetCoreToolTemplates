using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ManagementTasksOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("management-tasks", "Add task execution, based on command-line arguments.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
    }

    private static string GetPackageName(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Management.TaskCore" : "Steeltoe.Management.Tasks";
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
        snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
    }

    private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Management.TaskCore" : "Steeltoe.Management.Tasks";
    }

    private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return @"builder.Services.AddTask(""run-me"", _ =>";
            yield return "// Run this app with command-line argument: runtask=run-me";
            yield return @"Console.WriteLine(""Hello from application task."");";
            yield return "app.RunWithTasks();";
        }
        else
        {
            yield return @"builder.Services.AddTask(""run-me"", (_, _) =>";
            yield return "// Run this app with command-line argument: runtask=run-me";
            yield return @"Console.WriteLine(""Hello from application task."");";
            yield return "await app.RunWithTasksAsync(CancellationToken.None);";
        }
    }
}
