using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class FrameworkParameterTest : ChoiceParameterTest
    {
        public FrameworkParameterTest(ITestOutputHelper logger) : base("framework", "The target framework for the project.", logger)
        {
            Values.Add("net6.0");
            Values.Add("net8.0");
            Values.Add("net9.0");
            Values.Add("net10.0");
        }
    }
}
