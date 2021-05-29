using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class MySqlOptionTest : OptionTest
    {
        public MySqlOptionTest(ITestOutputHelper logger) : base("mysql", "Add access to MySQL databases", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("MySql.Data", "8.0.*"));
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

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.MySql;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.MySql;");
                    break;
            }

            snippets.Add("services.AddMySqlConnection(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
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
