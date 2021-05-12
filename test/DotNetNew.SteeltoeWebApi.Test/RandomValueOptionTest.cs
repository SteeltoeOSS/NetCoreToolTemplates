using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class RandomValueOptionTest : Test
    {
        public RandomValueOptionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --random-value  Add a random value configuration source.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestDefault()
        {
            using var sandbox = await TemplateSandbox();
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var packageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                where e.Value.Equals("Steeltoe.Extensions.Configuration.RandomValueBase")
                select e
            ).ToList();
            packageRefs.Count.Should().Be(0);
        }

        [Fact]
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox("--random-value");
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var packageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                where e.Value.Equals("Steeltoe.Extensions.Configuration.RandomValueBase")
                select e
            ).ToList();
            packageRefs.Count.Should().Be(1);
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox("--random-value");
            var valuesController = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            valuesController.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            valuesController.Should().ContainSnippet("private readonly IConfiguration _configuration;");
            valuesController.Should().ContainSnippet(@"
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
