using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMongoDbOptionTest : ProjectOptionTest
    {
        public ConnectorMongoDbOptionTest(ITestOutputHelper logger) : base("connector-mongodb",
            "Add a connector for MongoDB databases",
            logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("MongoDB.Driver", "2.8.*"));
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.MongoDb");
            snippets.Add("services.AddMongoClient");
        }
    }
}
