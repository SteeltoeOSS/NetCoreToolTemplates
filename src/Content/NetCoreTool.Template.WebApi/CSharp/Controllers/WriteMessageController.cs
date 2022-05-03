#if (MessagingRabbitMqOption || MessagingRabbitMqClientOption)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace Company.WebApplication.CS
{
    [ApiController]
    [Route("[controller]")]
    public class WriteMessageController : ControllerBase
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        private readonly ILogger<WriteMessageController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public WriteMessageController(ILogger<WriteMessageController> logger, RabbitTemplate rabbitTemplate, RabbitAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            var msg = "Hi there from over here.";

            _rabbitTemplate.ConvertAndSend(RECEIVE_AND_CONVERT_QUEUE, msg);

            _logger.LogInformation($"Sending message '{msg}' to queue '{RECEIVE_AND_CONVERT_QUEUE}'");

            return "Message sent to queue.";
        }
    }
}
#endif