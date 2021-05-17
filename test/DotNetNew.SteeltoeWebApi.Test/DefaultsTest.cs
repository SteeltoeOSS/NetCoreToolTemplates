using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DefaultsTest : Test
    {
        public DefaultsTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
Steeltoe Web API (C#)
Author: VMware
");
        }

        [Fact]
        public async void TestProject()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists($"{sandbox.Name}.csproj").Should().BeTrue();
        }

        [Fact]
        public async void TestProgram()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Program.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestStartup()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Startup.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestControllers()
        {
            using var sandbox = await TemplateSandbox();
            var fileText = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            fileText.Should().ContainSnippet("public ActionResult<string> Get(int id) { return \"value\"; }");
        }

        [Fact]
        public async void TestProperties()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Properties/launchSettings.json").Should().BeTrue();
        }

        [Fact]
        public async void TestSettings()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("appsettings.json").Should().BeTrue();
            sandbox.FileExists("appsettings.Development.json").Should().BeTrue();
        }

        [Fact]
        public async void TestConfig()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("app.config").Should().BeTrue();
        }
    }
}
