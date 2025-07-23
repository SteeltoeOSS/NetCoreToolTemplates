using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class CircuitBreakerHystrixOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("circuit-breaker-hystrix", "Add support for Netflix Hystrix, a latency and fault tolerance library (Steeltoe 3.x only).", logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("HelloHystrixCommand.cs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                Logger.WriteLine("asserting HelloHystrixCommand");
                var sourceFile = GetSourceFileForLanguage("HelloHystrixCommand", options.Language);
                var source = await Sandbox.GetFileTextAsync(sourceFile);
                source.Should().ContainSnippet("class HelloHystrixCommand");
            }
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Steeltoe.CircuitBreaker.HystrixCore", "$(SteeltoeVersion)"));
                packages.Add(("Steeltoe.CircuitBreaker.Hystrix.MetricsStreamCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                snippets.Add("using Steeltoe.CircuitBreaker.Hystrix;");
                snippets.Add($"using {Sandbox.Name};");

                snippets.Add(@"builder.Services.AddHystrixCommand<HelloHystrixCommand>(""ExampleCircuitBreakers"", builder.Configuration);");
                snippets.Add("builder.Services.AddHystrixMetricsStream(builder.Configuration);");
                snippets.Add("app.UseHystrixRequestContext();");
            }
        }
    }
}
