using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorRabbitMqOptionTest : ProjectOptionTest
    {
        public ConnectorRabbitMqOptionTest(ITestOutputHelper logger) : base("connector-rabbitmq",
            "Add a connector for RabbitMQ message brokers", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("RabbitMQ.Client", "5.1.*"));
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.CloudFoundry.Connector.RabbitMQ;");
            }
            else
            {
                snippets.Add("using Steeltoe.Connector.RabbitMQ;");
            }

            snippets.Add("services.AddRabbitMQConnection(Configuration);");
        }
    }
}
