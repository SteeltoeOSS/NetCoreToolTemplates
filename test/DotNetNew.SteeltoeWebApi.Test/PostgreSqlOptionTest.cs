using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PostgreSqlOptionTest : OptionTest
    {
        public PostgreSqlOptionTest(ITestOutputHelper logger) : base("postgresql", "Add access to PostgreSQL databases",
            logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Npgsql");
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
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.PostgreSql;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.PostgreSql;");
                    break;
            }

            snippets.Add("services.AddPostgresConnection(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
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
