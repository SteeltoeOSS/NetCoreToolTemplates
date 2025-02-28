using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorRabbitMqOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-rabbitmq", "Add a connector for RabbitMQ message brokers.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            var rabbitMqVersion = options.SteeltoeVersion switch
            {
                SteeltoeVersion.Steeltoe32 => "5.2.*",
                _ => "7.1.*"
            };

            packages.Add(("RabbitMQ.Client", rabbitMqVersion));
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
            return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Connector.RabbitMQ" : "Steeltoe.Connectors.RabbitMQ";
        }

        private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "// TODO: Add your connection string at configuration key: RabbitMq:Client:Url"
                : "// TODO: Add your connection string at configuration key: Steeltoe:Client:RabbitMQ:Default:ConnectionString";
        }

        private static string GetSetupCodeFragment(SteeltoeVersion steeltoeVersion)
        {
            return steeltoeVersion == SteeltoeVersion.Steeltoe32
                ? "builder.Services.AddRabbitMQConnection(builder.Configuration);"
                : "builder.AddRabbitMQ();";
        }
    }
}
