using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorOAuthOptionTest : ProjectOptionTest
    {
        public ConnectorOAuthOptionTest(ITestOutputHelper logger) : base("connector-oauth",
            "Add a connector for OAuth security",
            logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "3.1.*"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.OAuth");
            snippets.Add("services.AddOAuthServiceOptions");
        }
    }
}
