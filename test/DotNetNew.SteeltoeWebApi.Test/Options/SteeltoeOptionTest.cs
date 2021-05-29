using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class SteeltoeOptionTest : OptionTest
    {
        public SteeltoeOptionTest(ITestOutputHelper logger) : base("steeltoe",
            "Set the Steeltoe version for the project", logger)
        {
            SkipProjectGeneration = true;
            SmokeTestOption = "3.0.*";
        }
    }
}
