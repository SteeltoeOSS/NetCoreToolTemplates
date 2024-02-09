using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorPostgreSqlOptionTest : ProjectOptionTest
    {
        public ConnectorPostgreSqlOptionTest(ITestOutputHelper logger) : base("connector-postgresql",
            "Add a connector for PostgreSQL databases",
            logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Npgsql", "6.0.*"));
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.PostgreSql");
            snippets.Add("builder.Services.AddPostgresConnection");
        }
    }
}
