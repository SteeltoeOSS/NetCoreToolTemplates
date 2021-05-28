using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class CloudConfigOptionTest : OptionTest
    {
        public CloudConfigOptionTest(ITestOutputHelper logger) : base("cloud-config",
            "Add client support for Spring Cloud Config", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> packages)
        {
            packages.Add("Steeltoe.Extensions.Configuration.ConfigServerCore");
        }

        protected override void AddProgramCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.ConfigServer;");
            snippets.Add(".AddConfigServer()");
        }

        protected override void AddValuesControllerCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    var val1 = _configuration[""Value1""];
    var val2 = _configuration[""Value2""];
    return new[] { val1, val2 };
}
");
        }
    }
}
