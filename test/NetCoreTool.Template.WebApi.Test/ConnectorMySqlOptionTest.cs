using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlOptionTest : ProjectOptionTest
    {
        public ConnectorMySqlOptionTest(ITestOutputHelper logger) : base("connector-mysql",
            "Add a connector for MySQL databases", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("MySql.Data", "8.0.*"));
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add(" Steeltoe.Connector.MySql");
            snippets.Add("services.AddMySqlConnection");
        }
    }
}
