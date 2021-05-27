using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class SqlServerOptionTest : OptionTest
    {
        public SqlServerOptionTest(ITestOutputHelper logger) : base("sqlserver", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--sqlserver  Add access to Microsoft SQL Server.
             bool - Optional
             Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            var expectedPackageRefs = new List<string>
            {
                "System.Data.SqlClient",
            };
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    expectedPackageRefs.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    expectedPackageRefs.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
            packageRefs.Should().Contain(expectedPackageRefs);
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("using Steeltoe.CloudFoundry.Connector.SqlServer;");
                    break;
                default:
                    source.Should().ContainSnippet("using Steeltoe.Connector.SqlServer;");
                    break;
            }

            source.Should().ContainSnippet("services.AddSqlServerConnection(Configuration);");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using System.Data;");
            source.Should().ContainSnippet("using System.Data.SqlClient;");
            source.Should().ContainSnippet("private readonly SqlConnection _dbConnection;");
            source.Should().ContainSnippet(@"
private readonly SqlConnection _dbConnection;
public ValuesController([FromServices] SqlConnection dbConnection)
{
    _dbConnection = dbConnection;
}
");
            source.Should().ContainSnippet(@"
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
