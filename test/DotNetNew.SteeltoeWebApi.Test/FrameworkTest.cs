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
    }
}
