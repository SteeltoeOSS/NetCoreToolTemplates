using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudFoundryOptionTest : Test

    {
        public CloudFoundryOptionTest(ITestOutputHelper logger) : base("cloud-foundry", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--cloud-foundry  Add Cloud Foundry support.
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
                "Steeltoe.Common.Hosting",
                "Steeltoe.Extensions.Configuration.CloudFoundryCore",
                "Steeltoe.Extensions.Logging.DynamicLogger",
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
            programSource.Should().ContainSnippet("using Steeltoe.Common.Hosting;");
            programSource.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            programSource.Should().ContainSnippet(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should().ContainSnippet("Steeltoe.Extensions.Configuration.CloudFoundry;");
            programSource.Should().ContainSnippet(".ConfigureCloudFoundryOptions(Configuration);");
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
                string appName = _appOptions.ApplicationName;
                string appInstance = _appOptions.ApplicationId;
                return new[] { appInstance, appName };
            }
");
        }
    }
}
