using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RandomValueOptionTest : Test
    {
        public RandomValueOptionTest(ITestOutputHelper logger) : base("random-value", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--random-value  Add a random value configuration source.
                bool - Optional
                Default: false
");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
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
