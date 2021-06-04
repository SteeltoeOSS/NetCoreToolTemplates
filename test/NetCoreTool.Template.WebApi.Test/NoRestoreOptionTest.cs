using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class NoRestoreOptionTest : OptionTest
    {
        public NoRestoreOptionTest(ITestOutputHelper logger) : base("no-restore",
            "Skip the automatic restore of the project on create", logger)
        {
        }

        [Theory]
        [Trait("Category", "ProjectGeneration")]
        [InlineData("true")]
        [InlineData("false")]
        public async void TestObjDirectory(string trueOrFalse)
        {
            using var sandbox = await TemplateSandbox(trueOrFalse);
            sandbox.DirectoryExists("obj").Should().Be(trueOrFalse.Equals("false"));
        }
    }
}
