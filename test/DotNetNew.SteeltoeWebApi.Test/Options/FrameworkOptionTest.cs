using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class FrameworkOptionTest : OptionTest
    {
        public FrameworkOptionTest(ITestOutputHelper logger) : base("framework",
            "Set the target framework for the project", logger)
        {
            SkipProjectGeneration = true;
            SmokeTestOption = "net5.0";
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedFramework()
        {
            using var sandbox = await TemplateSandbox("unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --framework");
        }
    }
}
