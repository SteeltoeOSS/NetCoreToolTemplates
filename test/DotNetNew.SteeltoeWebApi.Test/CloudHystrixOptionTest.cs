using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudHystrixOptionTest : Test
    {
        public CloudHystrixOptionTest(ITestOutputHelper logger) : base("cloud-hystrix", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--cloud-hystrix  Add support for circuit breakers using Spring Cloud Netflix Hystrix.
                 bool - Optional
                 Default: false
");
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should()
                .ContainSnippet(
                    "services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            source.Should().ContainSnippet("services.AddHystrixMetricsStream(Configuration);");
            source.Should().ContainSnippet("app.UseHystrixRequestContext();");
            source.Should().ContainSnippet("app.UseHystrixMetricsStream();");
        }

        [Fact]
        public async void TestStartupCsNetCoreapp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should()
                .ContainSnippet(
                    "services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            source.Should().ContainSnippet("services.AddHystrixMetricsStream(Configuration);");
            source.Should().ContainSnippet("app.UseHystrixRequestContext();");
            source.Should().ContainSnippet("app.UseHystrixMetricsStream();");
        }

        [Fact]
        public async void TestHelloHystrixCommandCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("HelloHystrixCommand.cs");
            source.Should().ContainSnippet("public sealed class HelloHystrixCommand : HystrixCommand<string>");
            source.Should()
                .ContainSnippet(
                    "public HelloHystrixCommand(string name) : base(HystrixCommandGroupKeyDefault.AsKey(\"MyCircuitBreakers\"))");
        }
    }
}
