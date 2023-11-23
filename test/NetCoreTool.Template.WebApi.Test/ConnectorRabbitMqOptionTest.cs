using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorRabbitMqOptionTest : ProjectOptionTest
    {
        public ConnectorRabbitMqOptionTest(ITestOutputHelper logger) : base("connector-rabbitmq",
            "Add a connector for RabbitMQ message brokers", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("RabbitMQ.Client", "6.4.*"));
            packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Connector.RabbitMQ");
            snippets.Add("builder.Services.AddRabbitMQConnection");
        }
    }
}
