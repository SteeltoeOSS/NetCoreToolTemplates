using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class FrameworkOptionTest : OptionTest
    {
        public FrameworkOptionTest(ITestOutputHelper logger) : base("framework", logger)
        {
            SkipProjectGeneration = true;
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
-f|--framework  Set the target framework for the project.
                  net5.0
                  netcoreapp3.1
                  netcoreapp2.1
                Default: net5.0
");
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedFramework()
        {
            using var sandbox = await TemplateSandbox("unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --framework");
        }
    }
}
