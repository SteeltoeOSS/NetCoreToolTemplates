using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class SteeltoeParameterTest : ParameterTest
    {
        public SteeltoeParameterTest(ITestOutputHelper logger) : base("steeltoe",
            "Set the Steeltoe version for the project", logger)
        {
            Values.Add("3.0.*");
        }
    }
}
