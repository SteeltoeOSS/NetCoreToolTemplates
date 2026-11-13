using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-mysql", "Add a connector for MySQL databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("MySql.Data", "26.7.*"));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.MySql;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString");
            snippets.Add("builder.AddMySql();");
        }
    }
}
