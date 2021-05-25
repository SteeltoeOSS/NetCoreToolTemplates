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
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("services.AddDiscoveryClient(Configuration);");
            source.Should().ContainSnippet("app.UseDiscoveryClient();");
        }

        [Fact]
        public async void TestStartupCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("services.AddDiscoveryClient(Configuration);");
            source.Should().ContainSnippet("app.UseDiscoveryClient();");
        }
    }
}
