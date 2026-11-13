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
            packages.Add(("RabbitMQ.Client", "7.2.*"));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.RabbitMQ;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:RabbitMQ:Default:ConnectionString");
            snippets.Add("builder.AddRabbitMQ();");
        }
    }
}
