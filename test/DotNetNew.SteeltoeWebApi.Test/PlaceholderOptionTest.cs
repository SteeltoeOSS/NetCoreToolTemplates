using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PlaceholderOptionTest : Test
    {
        public PlaceholderOptionTest(ITestOutputHelper logger) : base("placeholder", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--placeholder  Add a placeholder configuration source.
               bool - Optional
               Default: false
");
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.Placeholder;");
            source.Should().ContainSnippet(".AddPlaceholderResolver()");
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
                var val1 = _configuration[""ResolvedPlaceholderFromEnvVariables""];
                var val2 = _configuration[""UnresolvedPlaceholder""];
                var val3 = _configuration[""ResolvedPlaceholderFromJson""];
                return new[] { val1, val2, val3 };
            }
");
        }

        [Fact]
        public async void TestAppSettingsJson()
        {
            using var sandbox = await TemplateSandbox();
            var settings = await sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json");
            settings.ResolvedPlaceholderFromEnvVariables.Should().Be("${PATH?NotFound}");
            settings.ResolvedPlaceholderFromJson.Should()
                .Be("${Logging:LogLevel:System?${Logging:LogLevel:Default}}");
            settings.UnresolvedPlaceholder.Should().Be("${SomKeyNotFound?NotFound}");
        }
    }
}
