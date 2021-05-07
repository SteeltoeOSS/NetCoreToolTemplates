using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class FrameworkTest : Test
    {
        public FrameworkTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
 -f|--framework  The target framework for the project.
                     netcoreapp3.1
                     netcoreapp2.1
                 Default: netcoreapp3.1
");
        }

        [Theory]
        [InlineData("netcoreapp3.1")]
        [InlineData("netcoreapp2.1")]
        public async void TestFramework(string framework)
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            var xDoc = await sandbox.GetXmlDocument($"{sandbox.Name}.csproj");
            var steeltoeVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("TargetFramework")
                select e
            ).ToArray();
            steeltoeVersions.Length.Should().Be(1);
            steeltoeVersions[0].Should().HaveValue(framework);
        }

        [Fact]
        public async void TestUnsupportedFramework()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework netcoreapp2.0");
            sandbox.CommandError.Should().Contain("'netcoreapp2.0' is not a valid value");
        }
    }
}
