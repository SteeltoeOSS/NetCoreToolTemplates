using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class NoRestoreOptionTest : Test
    {
        public NoRestoreOptionTest(ITestOutputHelper logger) : base("no-restore", logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --no-restore    If specified, skips the automatic restore of the project on create.
                  bool - Optional
                  Default: false
");
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async void TestObjDirectory(string trueOrFalse)
        {
            using var sandbox = await TemplateSandbox(trueOrFalse);
            sandbox.DirectoryExists("obj").Should().Be(trueOrFalse.Equals("false"));
        }
    }
}
