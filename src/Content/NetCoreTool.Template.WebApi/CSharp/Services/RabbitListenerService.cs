#if (MessagingRabbitMqListener)
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Company.WebApplication.CS.Services
{
    public class RabbitListenerService
    {
        private ILogger _logger;

        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        [RabbitListener("steeltoe_message_queue")]
        public void ListenForAMessage(string msg)
        {
            _logger.LogInformation($"Received the message '{msg}' from the queue.");
        }
    }
}
#endif
