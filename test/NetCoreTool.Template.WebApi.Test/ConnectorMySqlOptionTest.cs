using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorMySqlOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-mysql", "Add a connector for MySQL databases using ADO.NET.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            var mySqlVersion = options.Framework switch
            {
                Framework.Net60 => "9.1.*",
                _ => "9.3.*"
            };

            packages.Add(("MySql.Data", mySqlVersion));
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
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.MySql" : "Steeltoe.Connectors.MySql";
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: MySql:Client:ConnectionString"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "builder.Services.AddMySqlConnection(builder.Configuration);"
                : "builder.AddMySql();";
        }
    }
}
