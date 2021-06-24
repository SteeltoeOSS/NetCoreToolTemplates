#if (MessagingRabbitMqOption)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;
using Company.WebApplication1.Models;

namespace Company.WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMessagesController : ControllerBase
    {
        private readonly ILogger<RabbitMessagesController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public RabbitMessagesController(ILogger<RabbitMessagesController> logger,
            RabbitTemplate rabbitTemplate, RabbitAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            return @"
You can use these endpoints to interact with RabbitMQ
  /RabbitMessages/SendMessage
  /RabbitMessages/SendSpecialMessage
  /RabbitMessages/SendReceiveMessage
  /RabbitMessages/SendReceiveSpecialMessage
  /RabbitMessages/DeleteQueues
";
        }

        [HttpGet("SendMessage")]
        public ActionResult<string> SendMessage()
        {
            var message = new Message("my message");
            _rabbitTemplate.ConvertAndSend(Queues.InferredMessageQueue, message);
            _logger.LogInformation("SendMessage: sent message to {Queue}", Queues.InferredMessageQueue);
            return "Message sent ... look at logs to see if message processed by RabbitMessagesListenerService";
        }

        [HttpGet("SendSpecialMessage")]
        public ActionResult<string> SendSpecialMessage()
        {
            var message = new SpecialMessage("my special message");
            _rabbitTemplate.ConvertAndSend(Queues.InferredSpecialMessageQueue, message);
            _logger.LogInformation("SendSpecialMessage: sent message to {Queue}", Queues.InferredSpecialMessageQueue);
            return "Message sent ... look at logs to see if message processed by RabbitMessagesListenerService";
        }

        [HttpGet("SendAndReceiveMessage")]
        public ActionResult<string> SendAndReceiveMessage()
        {
            var message = new Message("my message");
            _rabbitTemplate.ConvertAndSend(Queues.ReceiveAndConvertQueue, message);
            _logger.LogInformation("SendAndReceiveMessage: sent message to {Queue} -> {Message}",
                Queues.ReceiveAndConvertQueue, message.Value);
            message = _rabbitTemplate.ReceiveAndConvert<Message>(Queues.ReceiveAndConvertQueue, 10_000);
            _logger.LogInformation("SendAndReceiveMessage: received message from {Queue} <- {Message}",
                Queues.ReceiveAndConvertQueue, message.Value);
            return message.ToString();
        }

        [HttpGet("SendAndReceiveSpecialMessage")]
        public ActionResult<string> SendAndReceiveSpecialMessage()
        {
            var message = new SpecialMessage("my special message");
            _rabbitTemplate.ConvertAndSend(Queues.ReceiveAndConvertQueue, message);
            _logger.LogInformation("SendAndReceiveSpecialMessage: sent special message to {Queue} -> {Message}",
                Queues.ReceiveAndConvertQueue, message.Value);
            message = _rabbitTemplate.ReceiveAndConvert<SpecialMessage>(Queues.ReceiveAndConvertQueue, 10_000);
            _logger.LogInformation("SendAndReceiveSpecialMessage: received message from {Queue} <- {Message} ",
                Queues.ReceiveAndConvertQueue, message.Value);
            return message.ToString();
        }

        [HttpGet("DeleteQueues")]
        public ActionResult<string> DeleteQueues()
        {
            _rabbitAdmin.DeleteQueue(Queues.ReceiveAndConvertQueue);
            _logger.LogInformation("DeleteQueue: Deleted queue {Queue}", Queues.ReceiveAndConvertQueue);
            return ("Queue deleted");
        }
    }
}
#endif
