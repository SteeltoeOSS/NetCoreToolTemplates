using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class SteeltoeVersionTest : Test
    {
        public SteeltoeVersionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
-s|--steeltoe  The Steeltoe version.
                   3.0.2
                   2.5.3
               Default: 3.0.2
");
        }

        [Theory]
        [InlineData("3.0.2")]
        [InlineData("2.5.3")]
        public async void TestSteeltoeVersion(string version)
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --steeltoe {version}");
            var xDoc = await sandbox.GetXmlDocument($"{sandbox.Name}.csproj");
            var steeltoeVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("SteeltoeVersion")
                select e
            ).ToArray();
            steeltoeVersions.Length.Should().Be(1);
            steeltoeVersions[0].Should().HaveValue(version);
        }

        [Fact]
        public async void TestUnsupportedSteeltoeVersion()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --steeltoe 1.2.3");
            sandbox.CommandError.Should().Contain("'1.2.3' is not a valid value");
        }
    }
}
