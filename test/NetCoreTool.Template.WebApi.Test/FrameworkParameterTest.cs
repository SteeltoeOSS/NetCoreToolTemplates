using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class FrameworkParameterTest : ChoiceParameterTest
    {
        public FrameworkParameterTest(ITestOutputHelper logger) : base("framework", "Set the target framework for the project", logger)
        {
            Values.Add("net6.0");
        }
    }
}
