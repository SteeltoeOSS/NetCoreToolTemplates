using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudFoundryOptionTest : OptionTest

    {
        public CloudFoundryOptionTest(ITestOutputHelper logger) : base("cloud-foundry",
            "Add hosting support for Cloud Foundry", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.Common.Hosting");
            packages.Add("Steeltoe.Extensions.Configuration.CloudFoundryCore");
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Common.Hosting;");
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            snippets.Add("services.ConfigureCloudFoundryOptions(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
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
