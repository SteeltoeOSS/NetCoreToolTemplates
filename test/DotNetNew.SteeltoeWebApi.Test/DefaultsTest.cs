using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class DefaultsTest : Test
    {
        public DefaultsTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestProject()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists($"{sandbox.Name}.csproj").Should().BeTrue();
        }

        [Fact]
        public async void TestProgram()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists("Program.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestStartup()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists("Startup.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestControllers()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            var fileText = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            fileText.Should().ContainSnippet("public ActionResult<string> Get(int id) { return \"value\"; }");
        }

        [Fact]
        public async void TestProperties()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists("Properties/launchSettings.json").Should().BeTrue();
        }

        [Fact]
        public async void TestSettings()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists("appsettings.json").Should().BeTrue();
            sandbox.FileExists("appsettings.Development.json").Should().BeTrue();
        }

        [Fact]
        public async void TestConfig()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileExists("app.config").Should().BeTrue();
        }
    }
}
