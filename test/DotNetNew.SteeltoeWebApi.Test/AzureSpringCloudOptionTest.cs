using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class AzureSpringCloudOptionTest : Test
    {
        public AzureSpringCloudOptionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --azure-spring-cloud  Add Microsoft Azure Spring Cloud support.
                        bool - Optional
                        Default: false
");
        }

        [Fact]
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox("--azure-spring-cloud");
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var packageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                where e.Value.Equals("Microsoft.Azure.SpringCloud.Client")
                select e
            ).ToList();
            packageRefs.Count.Should().Be(1);
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox("--azure-spring-cloud");
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Microsoft.Azure.SpringCloud.Client;");
            programSource.Should().ContainSnippet(".UseAzureSpringCloudService()");
        }
    }
}
