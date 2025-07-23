using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorPostgreSqlOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-postgresql", "Add a connector for PostgreSQL databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Npgsql", GetPackageVersionForFramework(options.Framework)));
            packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
        }

        private static string GetPackageName(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.ConnectorCore" : "Steeltoe.Connectors";
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
            snippets.Add(GetSetupComment(options.SteeltoeVersion));
            snippets.Add(GetSetupCodeFragment(options.SteeltoeVersion));
        }

        private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.PostgreSql" : "Steeltoe.Connectors.PostgreSql";
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: Postgres:Client:ConnectionString"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "builder.Services.AddPostgresConnection(builder.Configuration);"
                : "builder.AddPostgreSql();";
        }
    }
}
