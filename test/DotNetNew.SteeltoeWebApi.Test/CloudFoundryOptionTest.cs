using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudFoundryOptionTest : Test

    {
        public CloudFoundryOptionTest(ITestOutputHelper logger) : base("cloud-foundry", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--cloud-foundry  Add hosting support for Cloud Foundry.
                 bool - Optional
                 Default: false
");
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Steeltoe.Common.Hosting;");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            source.Should().ContainSnippet(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        [Fact]
        public async void TestStartupCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            source.Should().ContainSnippet("Steeltoe.Extensions.Configuration.CloudFoundry;");
            source.Should().ContainSnippet("services.ConfigureCloudFoundryOptions(Configuration);");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
            source.Should().ContainSnippet(@"
            [HttpGet]
            public ActionResult<IEnumerable<string>> Get()
            {
                string appName = _appOptions.ApplicationName;
                string appInstance = _appOptions.ApplicationId;
                return new[] { appInstance, appName };
            }
");
        }
    }
}
