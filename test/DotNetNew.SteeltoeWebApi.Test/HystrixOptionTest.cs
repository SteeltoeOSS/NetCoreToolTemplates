using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class HystrixOptionTest : OptionTest
    {
        public HystrixOptionTest(ITestOutputHelper logger) : base("hystrix",
            "Add support for Netflix Hystrix, a latency and fault tolerance library", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.CircuitBreaker.HystrixCore");
            packages.Add("Steeltoe.CircuitBreaker.Hystrix.MetricsStreamCore");
        }

        protected override async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            await base.AssertProject(steeltoe, framework);
            Logger.WriteLine("asserting HelloHystrixCommand.cs");
            var source = await Sandbox.GetFileTextAsync("HelloHystrixCommand.cs");
            source.Should().ContainSnippet("public sealed class HelloHystrixCommand : HystrixCommand<string>");
            source.Should()
                .ContainSnippet(
                    "public HelloHystrixCommand(string name) : base(HystrixCommandGroupKeyDefault.AsKey(\"MyCircuitBreakers\"))");
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            snippets.Add("services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            snippets.Add("services.AddHystrixMetricsStream(Configuration);");
            snippets.Add("app.UseHystrixRequestContext();");
            snippets.Add("app.UseHystrixMetricsStream();");
        }
    }
}
