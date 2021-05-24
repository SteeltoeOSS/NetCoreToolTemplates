using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class ActuatorOptionTest : Test
    {
        public ActuatorOptionTest(ITestOutputHelper logger) : base("actuator", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--actuator  Add endpoints to manage your application, such as health, metrics, etc.
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
                "Steeltoe.Management.EndpointCore",
            };
            var actualPackageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e.Value
            ).ToList();
            actualPackageRefs.Should().Contain(expectedPackageRefs);
        }

        [Fact]
        public async void TestCsprojSteeltoe2()
        {
            using var sandbox = await TemplateSandbox("--steeltoe 2.5.3");
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var expectedPackageRefs = new List<string>
            {
                "Steeltoe.Management.CloudFoundryCore",
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
            programSource.Should().ContainSnippet("Steeltoe.Management.Endpoint;");
            programSource.Should().ContainSnippet("services.AddAllActuators(Configuration);");
        }

        [Fact]
        public async void TestStartupCsSteeltoe2()
        {
            using var sandbox = await TemplateSandbox("--steeltoe 2.5.3");
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should().ContainSnippet("Steeltoe.Management.CloudFoundry;");
            programSource.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
            programSource.Should().ContainSnippet("app.UseCloudFoundryActuators()");
        }

        [Fact]
        public async void TestStartupCsSteeltoe2NetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--steeltoe 2.5.3 --framework netcoreapp2.1");
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should().ContainSnippet("Steeltoe.Management.CloudFoundry;");
            programSource.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
            programSource.Should().ContainSnippet("app.UseCloudFoundryActuators()");
        }
    }
}
