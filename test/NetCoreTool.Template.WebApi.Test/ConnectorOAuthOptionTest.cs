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
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }

            packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "3.1.*"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("Steeltoe.CloudFoundry.Connector.OAuth");
            }
            else
            {
                snippets.Add("Steeltoe.Connector.OAuth");
            }

            snippets.Add("services.AddOAuthServiceOptions");
        }
    }
}
