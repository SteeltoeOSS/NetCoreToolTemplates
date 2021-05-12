using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class PlaceholderOptionTest : Test
    {
        public PlaceholderOptionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --placeholder   Add a placeholder configuration source.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox("--placeholder");
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var packageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                where e.Value.Equals("Steeltoe.Extensions.Configuration.PlaceholderCore")
                select e
            ).ToList();
            packageRefs.Count.Should().Be(1);
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox("--placeholder");
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.Placeholder;");
            programSource.Should().ContainSnippet(".AddPlaceholderResolver()");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox("--placeholder");
            var valuesController = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            valuesController.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            valuesController.Should().ContainSnippet("private readonly IConfiguration _configuration;");
            valuesController.Should().ContainSnippet(@"
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
            using var sandbox = await TemplateSandbox("--placeholder");
            var launchSettings = await sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json");
            launchSettings.ResolvedPlaceholderFromEnvVariables.Should().Be("${PATH?NotFound}");
            launchSettings.ResolvedPlaceholderFromJson.Should()
                .Be("${Logging:LogLevel:System?${Logging:LogLevel:Default}}");
            launchSettings.UnresolvedPlaceholder.Should().Be("${SomKeyNotFound?NotFound}");
        }
    }
}
