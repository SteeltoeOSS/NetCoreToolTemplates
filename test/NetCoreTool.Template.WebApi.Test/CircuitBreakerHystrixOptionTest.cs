using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class CircuitBreakerHystrixOptionTest : ProjectOptionTest
    {
        public CircuitBreakerHystrixOptionTest(ITestOutputHelper logger) : base("circuit-breaker-hystrix",
            "Add support for Netflix Hystrix, a latency and fault tolerance library", logger)
        {
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async void TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("HelloHystrixCommand.cs").Should().BeFalse();
            sandbox.FileExists("HelloHystrixCommand.fs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine($"asserting HelloHystrixCommand");
            var sourceFile = GetSourceFileForLanguage("HelloHystrixCommand", options.Language);
            var source = await Sandbox.GetFileTextAsync(sourceFile);
            source.Should().ContainSnippet("HelloHystrixCommand");
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.CircuitBreaker.HystrixCore", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.CircuitBreaker.Hystrix.MetricsStreamCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("services.AddHystrixCommand<HelloHystrixCommand>");
            snippets.Add("services.AddHystrixMetricsStream");
            snippets.Add("app.UseHystrixRequestContext");
        }
    }
}
