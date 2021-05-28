using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class RabbitMqOptionTest : OptionTest
    {
        public RabbitMqOptionTest(ITestOutputHelper logger) : base("rabbitmq",
            "Add access to RabbitMQ message brokers", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> packages)
        {
            packages.Add("RabbitMQ.Client");
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.RabbitMQ;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.RabbitMQ;");
                    break;
            }

            snippets.Add("services.AddRabbitMQConnection(Configuration);");
        }

        protected override void AddValuesControllerCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
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
    using var connection = _factory.CreateConnection();
    using var channel = connection.CreateModel();
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
    return ""Wrote 5 message to the info log. Have a look!"";
");
        }
    }
}
