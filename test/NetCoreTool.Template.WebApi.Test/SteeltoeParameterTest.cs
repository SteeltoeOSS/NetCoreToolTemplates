using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class SteeltoeParameterTest : ParameterTest
    {
        public SteeltoeParameterTest(ITestOutputHelper logger) : base("steeltoe", "The Steeltoe version to use.", logger)
        {
            Values.Add("4.3.*");
            Values.Add("4.*-main-*");
        }
    }
}
