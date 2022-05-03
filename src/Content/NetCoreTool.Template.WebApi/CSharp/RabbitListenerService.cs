#if (MessagingRabbitMqOption || MessagingRabbitMqListenerOption)
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Company.WebApplication.CS
{
    public class RabbitListenerService
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        private ILogger _logger;

        public RabbitListenerService(ILogger<RabbitListenerService> logger)
        {
            _logger = logger;
        }

        [RabbitListener(RECEIVE_AND_CONVERT_QUEUE)]
        public void ListenForAMessage(string msg)
        {
            _logger.LogInformation($"Received the message '{msg}' from the queue.");
        }
    }
}
#endif