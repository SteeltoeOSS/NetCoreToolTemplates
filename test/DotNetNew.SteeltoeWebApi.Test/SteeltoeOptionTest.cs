using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class SteeltoeOptionTest : Test
    {
        public SteeltoeOptionTest(ITestOutputHelper logger) : base(logger)
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

        [Fact]
        public async void TestDefault()
        {
            using var sandbox = Sandbox();
            const string version = "3.0.2";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi");
            var expectedVersions = new List<string> { version };
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var actualVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("SteeltoeVersion")
                select e.Value
            ).ToList();
            actualVersions.Should().BeEquivalentTo(expectedVersions);
        }

        [Fact]
        public async void TestUnsupported()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --steeltoe unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --steeltoe");
        }

        [Theory]
        [InlineData("3.0.2")]
        [InlineData("2.5.3")]
        public async void TestCsproj(string option)
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --steeltoe {option}");
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var steeltoeVersions =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("SteeltoeVersion")
                select e
            ).ToArray();
            steeltoeVersions.Length.Should().Be(1);
            steeltoeVersions[0].Should().HaveValue(option);
        }
    }
}
