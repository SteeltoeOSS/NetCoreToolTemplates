using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class FrameworkParameterTest : ChoiceParameterTest
    {
        public FrameworkParameterTest(ITestOutputHelper logger) : base("framework",
            "<net5.0|net6.0> Set the target framework for the project", logger)
        {
            Values.Add("net5.0");
            Values.Add("netcoreapp2.1");
        }
    }
}
