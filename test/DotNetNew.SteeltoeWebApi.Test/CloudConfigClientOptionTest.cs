using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudConfigClientOptionTest : Test
    {
        public CloudConfigClientOptionTest(ITestOutputHelper logger) : base("cloud-config-client", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--cloud-config-client  Add Spring Cloud Config Client support.
                      bool - Optional
                      Default: false
");
        }

        [Fact]
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox();
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var expectedPackageRefs = new List<string>
            {
                "Steeltoe.Extensions.Configuration.ConfigServerCore",
            };
            var actualPackageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e.Value
            ).ToList();
            actualPackageRefs.Should().Contain(expectedPackageRefs);
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.ConfigServer;");
            programSource.Should().ContainSnippet(".AddConfigServer()");
        }

        [Fact]
        public async void TestProgramCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet(".AddConfigServer()");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox();
            var valuesController = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            valuesController.Should().ContainSnippet(@"
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
