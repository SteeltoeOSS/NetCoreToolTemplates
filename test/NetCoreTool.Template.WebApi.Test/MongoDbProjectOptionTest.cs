using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MongoDbProjectOptionTest : ProjectOptionTest
    {
        public MongoDbProjectOptionTest(ITestOutputHelper logger) : base("mongodb", "Add access to MongoDB databases", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("MongoDB.Driver", "2.8.*"));
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
                default:
                    packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.MongoDb;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.MongoDb;");
                    break;
            }

            snippets.Add("services.AddMongoClient(Configuration);");
        }

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using MongoDB.Driver;");
            // snippets.Add("using System.Data;");
            snippets.Add(@"
private readonly IMongoClient _mongoClient;
private readonly MongoUrl _mongoUrl;
public ValuesController(IMongoClient mongoClient, MongoUrl mongoUrl)
{
    _mongoClient = mongoClient;
    _mongoUrl = mongoUrl;
}
");
            snippets.Add(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    List<string> listing = _mongoClient.ListDatabaseNames().ToList();
    listing.Insert(0, _mongoUrl.Url);
    return listing;
}
");
        }
    }
}
