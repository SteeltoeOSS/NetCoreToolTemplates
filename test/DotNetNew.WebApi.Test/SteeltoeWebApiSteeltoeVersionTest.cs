using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class SteeltoeWebApiSteeltoeVersionTest : SteeltoeWebApiTest
    {
        public SteeltoeWebApiSteeltoeVersionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
-s|--steeltoe Steeltoe version
string - Optional
Default: 3.0.2");
        }

        [Fact]
        public async void TestSteeltoeVersion()
        {
            const string expectedVersion = "3.0.9";
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --steeltoe {expectedVersion}");
            var xDoc = await sandbox.GetXmlDocument($"{sandbox.Name}.csproj");
            var steeltoeVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("SteeltoeVersion")
                select e
            ).ToArray();
            steeltoeVersions.Length.Should().Be(1);
            steeltoeVersions[0].Should().HaveValue(expectedVersion);
        }
    }
}
