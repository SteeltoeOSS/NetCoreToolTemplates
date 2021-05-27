using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudConfigOptionTest : OptionTest
    {
        public CloudConfigOptionTest(ITestOutputHelper logger) : base("cloud-config",
            "Add client support for Spring Cloud Config", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.Extensions.Configuration.ConfigServerCore");
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Configuration.ConfigServer;");
            snippets.Add(".AddConfigServer()");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
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
