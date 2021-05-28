using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class MongoDbOptionTest : OptionTest
    {
        public MongoDbOptionTest(ITestOutputHelper logger) : base("mongodb", "Add access to MongoDB databases", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("MongoDB.Driver");
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.MongoDb;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.MongoDb;");
                    break;
            }

            snippets.Add("services.AddMongoClient(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
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
