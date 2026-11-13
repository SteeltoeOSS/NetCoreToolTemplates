using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ManagementTasksOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("management-tasks", "Add task execution, based on command-line arguments.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Steeltoe.Management.Tasks", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Steeltoe.Management.Tasks;");
        snippets.Add(@"builder.Services.AddTask(""run-me"", (_, _) =>");
        snippets.Add("// Run this app with command-line argument: runtask=run-me");
        snippets.Add(@"Console.WriteLine(""Hello from application task."");");
        snippets.Add("await app.RunWithTasksAsync(CancellationToken.None);");
    }
}
