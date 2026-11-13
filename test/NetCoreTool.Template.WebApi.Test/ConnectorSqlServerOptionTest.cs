using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorSqlServerOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-sqlserver", "Add a connector for Microsoft SQL Server databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Microsoft.Data.SqlClient", "6.1.*"));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.SqlServer;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString");
            snippets.Add("builder.AddSqlServer();");
        }
    }
}
