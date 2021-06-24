using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class RabbitMqProjectOptionTest : ProjectOptionTest
    {
        public RabbitMqProjectOptionTest(ITestOutputHelper logger) : base("rabbitmq",
            "Add access to RabbitMQ message brokers", logger)
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

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion,
            Framework framework,
            List<string> snippets)
        {
            snippets.Add("using RabbitMQ.Client;");
            snippets.Add("using RabbitMQ.Client.Events;");
            snippets.Add(@"
public ValuesController(ILogger<ValuesController> logger, [FromServices] ConnectionFactory factory)
{
    _logger = logger;
    _factory = factory;
}
");
            snippets.Add(@"
[HttpGet]
public ActionResult<string> Get()
{
    using (var connection = _factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        // the queue
        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        // consumer
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
                {
                    string msg = Encoding.UTF8.GetString(ea.Body);
                    _logger.LogInformation(""Received message: {Message}"", msg);
        };
        channel.BasicConsume(
            queue: QueueName,
            autoAck: true,
            consumer: consumer);
        // publisher
        int i = 0;
        while (i < 5) // write a message every second, for 5 seconds
        {
            var body = Encoding.UTF8.GetBytes($""Message {++i}"");
            channel.BasicPublish(
                exchange: """",
                routingKey: QueueName,
                basicProperties: null,
                body: body);
            Thread.Sleep(1000);
        }
    }
    return ""Wrote 5 message to the info log. Have a look!"";
");
        }
    }
}
