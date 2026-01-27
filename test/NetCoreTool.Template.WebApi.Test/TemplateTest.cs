using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities;
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
        public async Task Smoke_Test()
        {
            Logger.WriteLine("smoke testing help");
            using var helpBox = await TemplateSandbox("--help");

            if (Option != null)
            {
                var regex = @$"{Option}\s*(<[^>]*>)?\s+{EscapeSnippetForRegex(Description)}";
                helpBox.CommandOutput.Should().ContainRegexSnippet(regex);
            }
            else
            {
                helpBox.CommandOutput.Should().Contain(Description);
            }

            Logger.WriteLine("smoke testing option");
            using var smokeBox = await TemplateSandbox($"{GetSmokeTestArgs()}");
        }

        private static string EscapeSnippetForRegex(string snippet)
        {
            return snippet
                .Replace("(", @"\(").Replace(")", @"\)")
                .Replace("[", @"\[").Replace("]", @"\]")
                .Replace("|", @"\|")
                .Replace("+", @"\+")
                .Replace("$", @"\$")
                .Replace(".", @"\s*\.\s*");
        }

        protected abstract string GetSmokeTestArgs();

        protected virtual async Task<Sandbox> TemplateSandbox(string args = "", bool throwOnNonZeroExitCode = true)
        {
            var command = $"dotnet new steeltoe-webapi {args}".Trim();
            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command, throwOnNonZeroExitCode);
            return sandbox;
        }
    }
}
