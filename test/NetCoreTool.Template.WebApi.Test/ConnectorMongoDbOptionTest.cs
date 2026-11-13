using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMongoDbOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-mongodb", "Add a connector for MongoDB databases.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("MongoDB.Driver", "3.11.*"));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.MongoDb;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:MongoDb:Default:ConnectionString");
            snippets.Add("builder.AddMongoDb();");
        }
    }
}
