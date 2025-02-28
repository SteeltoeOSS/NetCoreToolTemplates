using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorOAuthOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-oauth", "Add a connector for OAuth2 security on Cloud Foundry (Steeltoe 3.x only).", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                snippets.Add("using Steeltoe.Connector.OAuth;");
                snippets.Add("builder.Services.AddOAuthServiceOptions(builder.Configuration);");
            }
        }
    }
}
