using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RabbitMqOptionTest : Test
    {
        public RabbitMqOptionTest(ITestOutputHelper logger) : base("rabbitmq", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--rabbitmq  Add support for RabbitMQ over AMQP.
            bool - Optional
            Default: false
");
        }

        [Theory]
        [InlineData("3.0.2")]
        [InlineData("2.5.3")]
        public async void TestCsproj(string steeltoe)
        {
            using var sandbox = await TemplateSandbox($"--steeltoe {steeltoe}");
            var project = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var expectedPackageRefs = steeltoe switch
            {
                "3.0.2" => new List<string>
                {
                    "Steeltoe.Connector.ConnectorCore",
                },
                "2.5.3" => new List<string>
                {
                    "Steeltoe.CloudFoundry.ConnectorCore",
                },
                _ => throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe)
            };
            var actualPackageRefs =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e
            ).ToList().Select(attr => attr.Value).ToList();
            actualPackageRefs.Should().Contain(expectedPackageRefs);
        }

        [Theory]
        [InlineData("3.0.2")]
        [InlineData("2.5.3")]
        public async void TestStartupCs(string steeltoe)
        {
            using var sandbox = await TemplateSandbox($"--steeltoe {steeltoe}");
            var source = await sandbox.GetFileTextAsync("Startup.cs");
            if (steeltoe.StartsWith("2.5."))
            {
                source.Should().ContainSnippet("using Steeltoe.CloudFoundry.Connector.RabbitMQ;");
            }
            else
            {
                source.Should().ContainSnippet("using Steeltoe.Connector.RabbitMQ;");
            }

            source.Should().ContainSnippet("services.AddRabbitMQConnection(Configuration);");
        }

        [Fact]
        public async void TestValuesController()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Controllers/ValuesController.cs");
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
