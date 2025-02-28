using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorPostgreSqlEfCoreOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-postgresql-efcore", "Add a connector for PostgreSQL databases using Entity Framework Core.", logger)
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
            packages.Add(("Npgsql.EntityFrameworkCore.PostgreSQL", GetPackageVersionForFramework(options.Framework)));
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
                yield return "using Steeltoe.Connector.PostgreSql.EFCore;";
            }
            else
            {
                yield return "using Steeltoe.Connectors.PostgreSql;";
                yield return "using Steeltoe.Connectors.EntityFrameworkCore.PostgreSql;";
            }
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: Postgres:Client:ConnectionString"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString";
        }

        private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
        {
            if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                yield return "builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration));";
            }
            else
            {
                yield return "builder.AddPostgreSql();";
                yield return "builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider));";
            }
        }
    }
}
