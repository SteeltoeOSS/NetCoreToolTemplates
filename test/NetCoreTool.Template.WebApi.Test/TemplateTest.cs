using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
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
        public async void Smoke_Test()
        {
            Logger.WriteLine($"smoke testing help");
            using var helpBox = await TemplateSandbox("--help");
            var regex = @$"{Option}\s*(<[^>]*>)?\s+{Description}";
            if (Option == null)
                helpBox.CommandOutput.Should()
                    .ContainSnippet(Description);
            else
                helpBox.CommandOutput.Should().MatchRegex(regex);
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
