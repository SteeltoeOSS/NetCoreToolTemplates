using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PostgreSqlProjectOptionTest : ProjectOptionTest
    {
        public PostgreSqlProjectOptionTest(ITestOutputHelper logger) : base("postgresql", "Add access to PostgreSQL databases",
            logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Npgsql", "4.1.*"));
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
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.PostgreSql;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.PostgreSql;");
                    break;
            }

            snippets.Add("services.AddPostgresConnection(Configuration);");
        }

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Npgsql;");
            snippets.Add("using System.Data;");
            snippets.Add(@"
private readonly NpgsqlConnection _dbConnection;
public ValuesController([FromServices] NpgsqlConnection dbConnection)
{
    _dbConnection = dbConnection;
}
");
            snippets.Add(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    List<string> tables = new List<string>();
    _dbConnection.Open();
    DataTable dt = _dbConnection.GetSchema(""Tables"");
    _dbConnection.Close();
    foreach (DataRow row in dt.Rows)
    {
        string tablename = (string)row[2];
        tables.Add(tablename);
    }

    return tables;
}
");
        }
    }
}
