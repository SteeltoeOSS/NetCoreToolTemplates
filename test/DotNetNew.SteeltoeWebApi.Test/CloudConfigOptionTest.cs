using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudConfigOptionTest : OptionTest
    {
        public CloudConfigOptionTest(ITestOutputHelper logger) : base("cloud-config", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet("--cloud-config  Add client support for Spring Cloud Config");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            packageRefs.Should().Contain("Steeltoe.Extensions.Configuration.ConfigServerCore");
        }

        protected override void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertProgramCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.ConfigServer;");
            source.Should().ContainSnippet(".AddConfigServer()");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet(@"
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
