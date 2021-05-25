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
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists($"{sandbox.Name}.csproj").Should().BeTrue();
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Program.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Startup.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestControllerCs()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Controllers/ValuesController.cs").Should().BeTrue();
        }

        [Fact]
        public async void TestLaunchSettingsJson()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("Properties/launchSettings.json").Should().BeTrue();
        }

        [Fact]
        public async void TestSettingsJson()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("appsettings.json").Should().BeTrue();
            sandbox.FileExists("appsettings.Development.json").Should().BeTrue();
        }

        [Fact]
        public async void TestAppConfig()
        {
            using var sandbox = await TemplateSandbox();
            sandbox.FileExists("app.config").Should().BeTrue();
        }
    }
}
