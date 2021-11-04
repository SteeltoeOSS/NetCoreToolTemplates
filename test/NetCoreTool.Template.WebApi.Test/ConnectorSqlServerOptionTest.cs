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
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("Steeltoe.CloudFoundry.Connector.SqlServer");
            }
            else
            {
                snippets.Add("Steeltoe.Connector.SqlServer");
            }

            snippets.Add("services.AddSqlServerConnection");
        }
    }
}
