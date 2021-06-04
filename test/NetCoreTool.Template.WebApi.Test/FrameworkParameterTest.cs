using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class FrameworkParameterTest : ChoiceParameterTest
    {
        public FrameworkParameterTest(ITestOutputHelper logger) : base("framework",
            "Set the target framework for the project", logger)
        {
            Values.Add("net5.0");
            Values.Add("netcoreapp3.1");
            Values.Add("netcoreapp2.1");
        }
    }
}
