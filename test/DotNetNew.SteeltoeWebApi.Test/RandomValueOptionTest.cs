using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RandomValueOptionTest : OptionTest
    {
        public RandomValueOptionTest(ITestOutputHelper logger) : base("random-value", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet("--random-value  Add a random value configuration source.");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            packageRefs.Should().Contain("Steeltoe.Extensions.Configuration.RandomValueBase");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            source.Should().ContainSnippet("private readonly IConfiguration _configuration;");
            source.Should().ContainSnippet(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    var val1 = _configuration[""random:int""];
    var val2 = _configuration[""random:uuid""];
    var val3 = _configuration[""random:string""];
    return new[] { val1, val2, val3 };
}
");
        }
    }
}
