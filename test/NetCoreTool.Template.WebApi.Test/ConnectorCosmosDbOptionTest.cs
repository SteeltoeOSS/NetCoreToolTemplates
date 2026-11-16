using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConnectorCosmosDbOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("connector-cosmosdb", "Add a connector for CosmosDB databases.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Microsoft.Azure.Cosmos", "3.64.*"));
        packages.Add(("Newtonsoft.Json", "13.0.*"));
        packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Steeltoe.Connectors.CosmosDb;");
        snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:CosmosDb:Default:ConnectionString");
        snippets.Add("builder.AddCosmosDb();");
    }
}
