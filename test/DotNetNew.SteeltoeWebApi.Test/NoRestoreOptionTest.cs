using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class NoRestoreOptionTest : OptionTest
    {
        public NoRestoreOptionTest(ITestOutputHelper logger) : base("no-restore", logger)
        {
            SkipProjectGeneration = true;
        }

        protected override void AssertHelp(string help)
        {
            help.Should().ContainSnippet("--no-restore  Skip the automatic restore of the project on create.");
        }

        [Theory]
        [Trait("Category", "Functional")]
        [InlineData("true")]
        [InlineData("false")]
        public async void TestObjDirectory(string trueOrFalse)
        {
            using var sandbox = await TemplateSandbox(trueOrFalse);
            sandbox.DirectoryExists("obj").Should().Be(trueOrFalse.Equals("false"));
        }
    }
}
