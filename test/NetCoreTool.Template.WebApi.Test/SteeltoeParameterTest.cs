using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class SteeltoeParameterTest : ParameterTest
    {
        public SteeltoeParameterTest(ITestOutputHelper logger) : base("steeltoe", "The Steeltoe version to use.", logger)
        {
            Values.Add("3.2.*");
            Values.Add("4.0.*-*");
            Values.Add("4.*-main-*");
        }
    }
}
