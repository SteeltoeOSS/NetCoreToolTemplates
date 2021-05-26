#if (RabbitMqConnector)
using System.Text;
using System.Threading;
#else
using System.Collections.Generic;
#endif
using Microsoft.AspNetCore.Mvc;
#if (CloudConfigClient || PlaceholderConfiguration || RandomValueConfiguration)
using Microsoft.Extensions.Configuration;
#endif
#if (RabbitMqConnector)
using Microsoft.Extensions.Logging;
#endif
#if (CloudFoundryHosting)
using Microsoft.Extensions.Options;
#endif
#if (RabbitMqConnector)
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
#endif
#if (CloudFoundryHosting)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif

namespace Company.WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
#if (CloudFoundryHosting)
        private readonly CloudFoundryApplicationOptions _appOptions;

        public ValuesController(IOptions<CloudFoundryApplicationOptions> appOptions)
        {
            _appOptions = appOptions.Value;
        }
#endif
#if (CloudConfigClient || PlaceholderConfiguration || RandomValueConfiguration)
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#endif
#if (RabbitMqConnector)
        private readonly ILogger _logger;
        private readonly ConnectionFactory _factory;
        private const string QueueName = "my-queue";

        public ValuesController(ILogger<ValuesController> logger, [FromServices] ConnectionFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }
#endif

        [HttpGet]
#if (RabbitMqConnector)
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
                _logger.LogInformation("Received message: {Message}", msg);
            };
            channel.BasicConsume(
                queue: QueueName,
                autoAck: true,
                consumer: consumer);
            // publisher
            int i = 0;
            while (i < 5) // write a message every second, for 5 seconds
            {
                var body = Encoding.UTF8.GetBytes($"Message {++i}");
                channel.BasicPublish(
                    exchange: "",
                    routingKey: QueueName,
                    basicProperties: null,
                    body: body);
                Thread.Sleep(1000);
            }

            return "Wrote 5 message to the info log. Have a look!";
#else
        public ActionResult<IEnumerable<string>> Get()
        {
#if (CloudConfigClient)
            var val1 = _configuration["Value1"];
            var val2 = _configuration["Value2"];

            return new[] { val1, val2 };
#elif (CloudFoundryHosting)
            string appName = _appOptions.ApplicationName;
            string appInstance = _appOptions.ApplicationId;

            return new[] { appInstance, appName };
#elif (PlaceholderConfiguration)
            var val1 = _configuration["ResolvedPlaceholderFromEnvVariables"];
            var val2 = _configuration["UnresolvedPlaceholder"];
            var val3 = _configuration["ResolvedPlaceholderFromJson"];

            return new[] { val1, val2, val3 };
#elif (RandomValueConfiguration)
            var val1 = _configuration["random:int"];
            var val2 = _configuration["random:uuid"];
            var val3 = _configuration["random:string"];

            return new[] { val1, val2, val3 };
#else
            return new[] { "value" };
#endif
#endif
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
