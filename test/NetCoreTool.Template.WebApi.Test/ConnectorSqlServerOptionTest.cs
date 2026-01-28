using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorSqlServerOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-sqlserver", "Add a connector for Microsoft SQL Server databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            var sqlClientVersion = options.Framework switch
            {
                Framework.Net60 => "5.2.*",
                Framework.Net80 => "6.0.*",
                Framework.Net90 => "6.0.*",
                Framework.Net100 => "6.1.*",
                _ => throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString())
            };

            packages.Add(("Microsoft.Data.SqlClient", sqlClientVersion));
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
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.SqlServer" : "Steeltoe.Connectors.SqlServer";
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: SqlServer:Credentials:ConnectionString"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "builder.Services.AddSqlServerConnection(builder.Configuration);"
                : "builder.AddSqlServer();";
        }
    }
}
