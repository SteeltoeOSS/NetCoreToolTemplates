using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class NoRestoreOptionTest(ITestOutputHelper logger)
        : OptionTest("no-restore", "If specified, skips the automatic restore of the project on create.", logger)
    {
        [Theory]
        [Trait("Category", "ProjectGeneration")]
        [InlineData("true")]
        [InlineData("false")]
        public async Task TestObjDirectory(string trueOrFalse)
        {
            using var sandbox = await TemplateSandbox(trueOrFalse);
            sandbox.DirectoryExists("obj").Should().Be(trueOrFalse.Equals("false"));
        }
    }
}
