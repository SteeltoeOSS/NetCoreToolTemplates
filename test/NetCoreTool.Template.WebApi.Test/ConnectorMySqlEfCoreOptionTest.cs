using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlEfCoreOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-mysql-efcore", "Add a connector for MySQL databases using Entity Framework Core.", logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("AppDbContext.cs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            Logger.WriteLine("asserting Models");
            Sandbox.FileExists(GetSourceFileForLanguage("AppDbContext", options.Language)).Should().BeTrue();
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Microsoft.EntityFrameworkCore", GetPackageVersionForFramework(options.Framework)));
            packages.Add(("MySql.EntityFrameworkCore", GetPackageVersionForFramework(options.Framework)));
            packages.Add(("Steeltoe.Connectors.EntityFrameworkCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {Sandbox.Name};");
            snippets.Add("using Steeltoe.Connectors.MySql;");
            snippets.Add("using Steeltoe.Connectors.EntityFrameworkCore.MySql;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString");
            snippets.Add("builder.AddMySql();");
            snippets.Add("builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));");
        }
    }
}
