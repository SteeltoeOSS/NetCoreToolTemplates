using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class NoRestoreOptionTest : Test
    {
        public NoRestoreOptionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandExactlyAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --no-restore    If specified, skips the automatic restore of the project on create.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestDefault()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandExactlyAsync($"dotnet new stwebapi");
            sandbox.DirectoryExists("obj").Should().BeTrue();
        }

        [Fact]
        public async void TestObj()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandExactlyAsync($"dotnet new stwebapi --no-restore");
            sandbox.DirectoryExists("obj").Should().BeFalse();
        }
    }
}
