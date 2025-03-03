using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class ParameterTest(string option, string description, ITestOutputHelper logger)
        : OptionTest(option, description, logger)
    {
        protected List<string> Values { get; init; } = [];

        protected override string GetSmokeTestArgs()
        {
            return Values[0];
        }
    }
}
