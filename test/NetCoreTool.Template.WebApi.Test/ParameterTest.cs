using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ParameterTest : OptionTest
    {
        protected List<string> Values { get; init; } = new();

        protected ParameterTest(string option, string description, ITestOutputHelper logger) : base(option,
            description, logger)
        {
        }

        protected override string GetSmokeTestArgs()
        {
            return Values[0];
        }
    }
}
