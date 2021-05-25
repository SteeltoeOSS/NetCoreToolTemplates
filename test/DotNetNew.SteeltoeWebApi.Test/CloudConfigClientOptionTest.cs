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
--cloud-config-client  Add client support for Spring Cloud Config.
                      bool - Optional
                      Default: false
");
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.ConfigServer;");
            source.Should().ContainSnippet(".AddConfigServer()");
        }

        [Fact]
        public async void TestProgramCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet(".AddConfigServer()");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
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
