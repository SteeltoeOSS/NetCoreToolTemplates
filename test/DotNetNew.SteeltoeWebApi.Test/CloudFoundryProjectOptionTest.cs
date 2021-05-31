using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudFoundryProjectOptionTest : ProjectOptionTest

    {
        public CloudFoundryProjectOptionTest(ITestOutputHelper logger) : base("cloud-foundry",
            "Add hosting support for Cloud Foundry", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Common.Hosting", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.Extensions.Configuration.CloudFoundryCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Common.Hosting;");
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add("services.ConfigureCloudFoundryOptions(Configuration);");
        }

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    string appName = _appOptions.ApplicationName;
    string appInstance = _appOptions.ApplicationId;
    return new[] { appInstance, appName };
}
");
        }
    }
}
