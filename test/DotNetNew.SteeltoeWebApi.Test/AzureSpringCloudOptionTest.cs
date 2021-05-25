using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class AzureSpringCloudOptionTest : Test
    {
        public AzureSpringCloudOptionTest(ITestOutputHelper logger) : base("azure-spring-cloud", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--azure-spring-cloud  Add hosting support for Microsoft Azure Spring Cloud.
                      bool - Optional
                      Default: false
");
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Microsoft.Azure.SpringCloud.Client;");
            source.Should().ContainSnippet(".UseAzureSpringCloudService()");
        }

        [Fact]
        public async void TestProgramCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().NotContainSnippet("using Microsoft.Azure.SpringCloud.Client;");
        }
    }
}
