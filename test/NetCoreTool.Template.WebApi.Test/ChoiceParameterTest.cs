using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ChoiceParameterTest(string option, string description, ITestOutputHelper logger)
        : ParameterTest(option, description, logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestUnsupportedParameterValue()
        {
            using var sandbox = await TemplateSandbox("UnsupportedValue", false);
            sandbox.CommandExitCode.Should().NotBe(0);
            sandbox.CommandOutput.Should().Contain($"'UnsupportedValue' is not a valid value for --{Option}");
        }
    }
}
