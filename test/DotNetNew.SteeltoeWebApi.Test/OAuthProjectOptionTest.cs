using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class OAuthProjectOptionTest : ProjectOptionTest
    {
        public OAuthProjectOptionTest(ITestOutputHelper logger) : base("oauth", "Add access to OAuth security", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
                default:
                    packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
            }

            switch (framework)
            {
                case Framework.NetCoreApp21:
                    packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "2.1.*"));
                    break;
                default:
                    packages.Add(("Microsoft.AspNetCore.Authentication.AzureAD.UI", "3.1.*"));
                    break;
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.OAuth;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.OAuth;");
                    break;
            }

            snippets.Add("services.AddOAuthServiceOptions(Configuration);");
        }
    }
}
