using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class SqlServerOptionTest : OptionTest
    {
        public SqlServerOptionTest(ITestOutputHelper logger) : base("sqlserver", "Add access to Microsoft SQL Server",
            logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("System.Data.SqlClient");
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
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.SqlServer;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.SqlServer;");
                    break;
            }

            snippets.Add("services.AddSqlServerConnection(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using System.Data;");
            snippets.Add("using System.Data.SqlClient;");
            snippets.Add("private readonly SqlConnection _dbConnection;");
            snippets.Add(@"
private readonly SqlConnection _dbConnection;
public ValuesController([FromServices] SqlConnection dbConnection)
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
