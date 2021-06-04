using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ChoiceParameterTest : ParameterTest
    {
        protected ChoiceParameterTest(string option, string description, ITestOutputHelper logger) : base(option, description, logger)
        {
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async void TestUnsupportedParameterValue()
        {
            using var sandbox = await TemplateSandbox("UnsupportedValue");
            sandbox.CommandOutput.Should().Contain($"'UnsupportedValue' is not a valid value for --{Option}");
        }
    }
}
