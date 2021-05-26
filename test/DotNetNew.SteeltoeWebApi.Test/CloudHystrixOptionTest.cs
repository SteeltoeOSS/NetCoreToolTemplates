using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudHystrixOptionTest : OptionTest
    {
        public CloudHystrixOptionTest(ITestOutputHelper logger) : base("cloud-hystrix", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--cloud-hystrix  Add support for circuit breakers using Spring Cloud Netflix Hystrix.
                 bool - Optional
                 Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            packageRefs.Should().Contain("Steeltoe.CircuitBreaker.HystrixCore");
            packageRefs.Should().Contain("Steeltoe.CircuitBreaker.Hystrix.MetricsStreamCore");
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

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            source.Should()
                .ContainSnippet(
                    "services.AddHystrixCommand<HelloHystrixCommand>(\"MyCircuitBreakers\", Configuration);");
            source.Should().ContainSnippet("services.AddHystrixMetricsStream(Configuration);");
            source.Should().ContainSnippet("app.UseHystrixRequestContext();");
            source.Should().ContainSnippet("app.UseHystrixMetricsStream();");
        }
    }
}
