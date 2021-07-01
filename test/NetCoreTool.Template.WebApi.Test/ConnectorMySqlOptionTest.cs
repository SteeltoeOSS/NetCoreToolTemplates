using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlOptionTest : ProjectOptionTest
    {
        public ConnectorMySqlOptionTest(ITestOutputHelper logger) : base("connector-mysql",
            "Add a connector for MySQL databases", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("MySql.Data", "8.0.*"));
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.CloudFoundry.Connector.MySql;");
            }
            else
            {
                snippets.Add("using Steeltoe.Connector.MySql;");
            }

            snippets.Add("services.AddMySqlConnection(Configuration);");
        }

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion,
            Framework framework,
            List<string> snippets)
        {
            snippets.Add("using MySql.Data.MySqlClient;");
            snippets.Add("using System.Data;");
            snippets.Add(@"
private readonly MySqlConnection _dbConnection;
public ValuesController([FromServices] MySqlConnection dbConnection)
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
