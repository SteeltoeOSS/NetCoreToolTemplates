using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities;
using Steeltoe.NetCoreTool.Template.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class TemplateTest
    {
        protected string Option { get; }

        private string Description { get; }

        protected Sandbox Sandbox { get; set; }

        protected readonly ITestOutputHelper Logger;

        protected TemplateTest(string option, string description, ITestOutputHelper logger)
        {
            Option = option;
            Description = description;
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void SmokeTest()
        {
            Logger.WriteLine($"smoke testing help");
            using var helpBox = await TemplateSandbox("--help");
            helpBox.CommandOutput.Should().ContainSnippet($"{Option} {Description}");
            Logger.WriteLine($"smoke testing option");
            using var smokeBox = await TemplateSandbox($"{GetSmokeTestArgs()}");
        }

        protected abstract string GetSmokeTestArgs();

        protected virtual async Task<Sandbox> TemplateSandbox(string args = "")
        {
            var command = $"dotnet new steeltoe-webapi {args}".Trim();
            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command);
            return sandbox;
        }
    }
}
