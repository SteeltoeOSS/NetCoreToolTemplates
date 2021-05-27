using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class SteeltoeOptionTest : OptionTest
    {
        public SteeltoeOptionTest(ITestOutputHelper logger) : base("steeltoe", logger)
        {
            SkipProjectGeneration = true;
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
-s|--steeltoe  Set the Steeltoe version for the project.
                 3.0.2
                 2.5.3
               Default: 3.0.2
");
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedSteeltoeVersion()
        {
            using var sandbox = await TemplateSandbox("--steeltoe unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --steeltoe");
        }
    }
}
