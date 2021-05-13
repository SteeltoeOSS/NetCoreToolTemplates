using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class SteeltoeOptionTest : Test
    {
        public SteeltoeOptionTest(ITestOutputHelper logger) : base("steeltoe", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
-s|--steeltoe  The Steeltoe version.
                 3.0.2
                 2.5.3
               Default: 3.0.2
");
        }

        [Fact]
        public async void TestUnsupported()
        {
            using var sandbox = await TemplateSandbox("unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --steeltoe");
        }

        [Theory]
        [InlineData("3.0.2")]
        [InlineData("2.5.3")]
        public async void TestCsproj(string steeltoe)
        {
            using var sandbox = await TemplateSandbox(steeltoe);
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var steeltoeVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("SteeltoeVersion")
                select e
            ).ToArray();
            steeltoeVersions.Length.Should().Be(1);
            steeltoeVersions[0].Should().HaveValue(steeltoe);
        }
    }
}
