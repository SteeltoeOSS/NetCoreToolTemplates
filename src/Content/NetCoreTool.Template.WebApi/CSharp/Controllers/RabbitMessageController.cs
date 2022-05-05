#if (MessagingRabbitMqClient)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace Company.WebApplication.CS.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMessageController : ControllerBase
    {
        private readonly ILogger<RabbitMessageController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public RabbitMessageController(ILogger<RabbitMessageController> logger, RabbitTemplate rabbitTemplate, RabbitAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            var msg = "Hi there from over here.";
            _rabbitTemplate.ConvertAndSend("steeltoe_message_queue", msg);
            _logger.LogInformation($"Sending message '{msg}' to queue 'steeltoe_message_queue'");
            return "Message sent to queue.";
        }
    }
}
#endif
