#if (MessagingRabbitMqOption)
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Company.WebApplication1.Models;

namespace Company.WebApplication1.Services
{
    public class RabbitMessagesListener
    {
        private ILogger _logger;

        public RabbitMessagesListener(ILogger<RabbitMessagesListener> logger)
        {
            _logger = logger;
        }

        [RabbitListener("my.message.queue")]
        public void ListenForMessage(Message message)
        {
            _logger.LogInformation("Got a message: {Message}", message.Value);
        }
    }
}
#endif
