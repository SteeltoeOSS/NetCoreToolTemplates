using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class OAuthOptionTest : OptionTest
    {
        public OAuthOptionTest(ITestOutputHelper logger) : base("oauth", "Add access to OAuth security", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Microsoft.AspNetCore.Authentication.AzureAD.UI");
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
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
