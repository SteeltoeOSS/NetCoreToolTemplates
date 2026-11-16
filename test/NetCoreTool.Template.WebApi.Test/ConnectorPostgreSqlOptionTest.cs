using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorPostgreSqlOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-postgresql", "Add a connector for PostgreSQL databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Npgsql", "10.0.*"));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.PostgreSql;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString");
            snippets.Add("builder.AddPostgreSql();");
        }
    }
}
