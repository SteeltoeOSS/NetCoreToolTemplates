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
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should()
                .ContainSnippet(
                    "services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            programSource.Should().ContainSnippet("services.AddHystrixMetricsStream(Configuration);");
            programSource.Should().ContainSnippet("app.UseHystrixRequestContext();");
            programSource.Should().ContainSnippet("app.UseHystrixMetricsStream();");
        }

        [Fact]
        public async void TestStartupCsNetCoreapp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var programSource = await sandbox.GetFileTextAsync("Startup.cs");
            programSource.Should()
                .ContainSnippet(
                    "services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            programSource.Should().ContainSnippet("services.AddHystrixMetricsStream(Configuration);");
            programSource.Should().ContainSnippet("app.UseHystrixRequestContext();");
            programSource.Should().ContainSnippet("app.UseHystrixMetricsStream();");
        }

        [Fact]
        public async void TestHelloHystrixCommandCs()
        {
            using var sandbox = await TemplateSandbox();
            var programSource = await sandbox.GetFileTextAsync("HelloHystrixCommand.cs");
            programSource.Should().ContainSnippet("public sealed class HelloHystrixCommand : HystrixCommand<string>");
            programSource.Should()
                .ContainSnippet(
                    "public HelloHystrixCommand(string name) : base(HystrixCommandGroupKeyDefault.AsKey(\"MyCircuitBreakers\"))");
        }
    }
}
