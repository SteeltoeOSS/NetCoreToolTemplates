using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class OAuthOptionTest : ProjectOptionTest
    {
        public OAuthOptionTest(ITestOutputHelper logger) : base("oauth", "Add access to OAuth security", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }

            if (framework < Framework.NetCoreApp31)
            {
                packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "2.1.*"));
            }
            else
            {
                packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "3.1.*"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.CloudFoundry.Connector.OAuth;");
            }
            else
            {
                snippets.Add("using Steeltoe.Connector.OAuth;");
            }

            snippets.Add("services.AddOAuthServiceOptions(Configuration);");
        }
    }
}
