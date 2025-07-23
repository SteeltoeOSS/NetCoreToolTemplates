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
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.EFCore" : "Steeltoe.Connectors.EntityFrameworkCore";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {Sandbox.Name};");
            snippets.AddRange(GetNamespaceImports(options.SteeltoeVersion));
            snippets.Add(GetSetupComment(options.SteeltoeVersion));
            snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
        }

        private static IEnumerable<string> GetNamespaceImports(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "using Steeltoe.Connector.MySql.EFCore;";
            }
            else
            {
                yield return "using Steeltoe.Connectors.MySql;";
                yield return "using Steeltoe.Connectors.EntityFrameworkCore.MySql;";
            }
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: MySql:Client:ConnectionString"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString";
        }

        private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration));";
            }
            else
            {
                yield return "builder.AddMySql();";
                yield return "builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));";
            }
        }
    }
}
