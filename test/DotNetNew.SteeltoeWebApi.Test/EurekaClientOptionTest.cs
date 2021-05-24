using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class EurekaClientOptionTest : Test
    {
        public EurekaClientOptionTest(ITestOutputHelper logger) : base("eureka-client", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--eureka-client  Add client support for Eureka, a REST-based service for locating services.
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
                "Steeltoe.Discovery.ClientCore",
            };
            var actualPackageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e.Value
            ).ToList();
            actualPackageRefs.Should().Contain(expectedPackageRefs);
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should().ContainSnippet("services.AddDiscoveryClient(Configuration);");
            programSource.Should().ContainSnippet("app.UseDiscoveryClient();");
        }

        [Fact]
        public async void TestStartupCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should().ContainSnippet("services.AddDiscoveryClient(Configuration);");
            programSource.Should().ContainSnippet("app.UseDiscoveryClient();");
        }
    }
}
