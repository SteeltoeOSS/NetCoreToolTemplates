using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class SteeltoeOptionTest : OptionTest
    {
        public SteeltoeOptionTest(ITestOutputHelper logger) : base("steeltoe",
            "Set the Steeltoe version for the project", logger)
        {
            SkipProjectGeneration = true;
            SmokeTestOption = "3.0.2";
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedSteeltoeVersion()
        {
            using var sandbox = await TemplateSandbox("unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --steeltoe");
        }
    }
}
