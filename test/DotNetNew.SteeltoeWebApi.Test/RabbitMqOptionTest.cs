using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RabbitMqOptionTest : OptionTest
    {
        public RabbitMqOptionTest(ITestOutputHelper logger) : base("rabbitmq", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet("--rabbitmq  Add access to RabbitMQ, an open source message broker.");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            var expectedPackageRefs = new List<string>
            {
                "RabbitMQ.Client",
            };
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe3:
                    expectedPackageRefs.Add("Steeltoe.Connector.ConnectorCore");
                    break;
                default:
                    expectedPackageRefs.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
            }
            packageRefs.Should().Contain(expectedPackageRefs);
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("using Steeltoe.CloudFoundry.Connector.RabbitMQ;");
                    break;
                default:
                    source.Should().ContainSnippet("using Steeltoe.Connector.RabbitMQ;");
                    break;
            }

            source.Should().ContainSnippet("services.AddRabbitMQConnection(Configuration);");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using RabbitMQ.Client;");
            source.Should().ContainSnippet("using RabbitMQ.Client.Events;");
            source.Should().ContainSnippet(@"
public ValuesController(ILogger<ValuesController> logger, [FromServices] ConnectionFactory factory)
{
    _logger = logger;
    _factory = factory;
}
");
            source.Should().ContainSnippet(@"
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
