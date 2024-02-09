using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorSqlServerOptionTest : ProjectOptionTest
    {
        public ConnectorSqlServerOptionTest(ITestOutputHelper logger) : base("connector-sqlserver",
            "Add a connector for Microsoft SQL Server databases",
            logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("System.Data.SqlClient", "4.8.*"));
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.SqlServer");
            snippets.Add("builder.Services.AddSqlServerConnection");
        }
    }
}
