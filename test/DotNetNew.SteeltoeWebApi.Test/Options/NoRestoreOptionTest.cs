using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class NoRestoreOptionTest : OptionTest
    {
        public NoRestoreOptionTest(ITestOutputHelper logger) : base("no-restore",
            "Skip the automatic restore of the project on create", logger)
        {
            SkipProjectGeneration = true;
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
