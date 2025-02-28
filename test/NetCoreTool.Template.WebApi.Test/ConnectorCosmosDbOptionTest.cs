using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConnectorCosmosDbOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("connector-cosmosdb", "Add a connector for CosmosDB databases.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Microsoft.Azure.Cosmos", "3.47.*"));
        packages.Add(("Newtonsoft.Json", "13.0.*"));
        packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
    }

    private static string GetPackageName(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.ConnectorCore" : "Steeltoe.Connectors";
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
        snippets.Add(GetSetupComment(options.SteeltoeVersion));
        snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
    }

    private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.CosmosDb" : "Steeltoe.Connectors.CosmosDb";
    }

    private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32
            ? "// TODO: Add your connection string at configuration key: CosmosDb:Client:ConnectionString"
            : "// TODO: Add your connection string at configuration key: Steeltoe:Client:CosmosDb:Default:ConnectionString";
    }

    private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return "var manager = new ConnectionStringManager(builder.Configuration);";
            yield return "var cosmosInfo = manager.Get<CosmosDbConnectionInfo>();";
        }
        else
        {
            yield return "builder.AddCosmosDb();";
        }
    }
}
