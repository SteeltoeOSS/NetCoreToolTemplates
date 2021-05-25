using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class ActuatorOptionTest : Test
    {
        public ActuatorOptionTest(ITestOutputHelper logger) : base("actuator", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--actuator  Add endpoints to manage your application, such as health, metrics, etc.
            bool - Optional
            Default: false
");
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("Steeltoe.Management.Endpoint;");
            source.Should().ContainSnippet("services.AddAllActuators(Configuration);");
        }

        [Fact]
        public async void TestStartupCsSteeltoe2()
        {
            using var sandbox = await TemplateSandbox("--steeltoe 2.5.3");
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("Steeltoe.Management.CloudFoundry;");
            source.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
            source.Should().ContainSnippet("app.UseCloudFoundryActuators()");
        }

        [Fact]
        public async void TestStartupCsSteeltoe2NetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--steeltoe 2.5.3 --framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("Steeltoe.Management.CloudFoundry;");
            source.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
            source.Should().ContainSnippet("app.UseCloudFoundryActuators()");
        }
    }
}
