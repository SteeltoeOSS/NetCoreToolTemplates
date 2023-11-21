using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlEfCoreOptionTest : ProjectOptionTest
    {
        public ConnectorMySqlEfCoreOptionTest(ITestOutputHelper logger) : base("connector-mysql-efcore",
            "Add a connector for MySQL databases using Entity Framework Core", logger)
        {
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async void TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeFalse();
            sandbox.FileExists("Models/SampleContext.cs").Should().BeFalse();
            sandbox.FileExists("Models/ErrorViewModel.fs").Should().BeFalse();
            sandbox.FileExists("Models/SampleContext.fs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Models/SampleContext");
            Sandbox.FileExists(GetSourceFileForLanguage("Models/SampleContext", options.Language)).Should().BeTrue();
            Sandbox.FileExists(GetSourceFileForLanguage("Models/ErrorViewModel", options.Language)).Should().BeTrue();
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Microsoft.EntityFrameworkCore", "3.1.*"));
            packages.Add(("Steeltoe.Connector.EFCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.MySql.EFCore");
            snippets.Add($"{Sandbox.Name}.Models");
            snippets.Add(".UseMySql");
        }
    }
}
