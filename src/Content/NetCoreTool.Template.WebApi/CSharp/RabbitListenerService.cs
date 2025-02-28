#if (HasMessagingRabbitMqListenerInSteeltoeV3)
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Company.WebApplication.CS;

public sealed class RabbitListenerService
{
    private readonly ILogger _logger;

    public RabbitListenerService(ILogger<RabbitListenerService> logger)
    {
        _logger = logger;
    }

    [RabbitListener("steeltoe_message_queue")]
    public void ListenForAMessage(string message)
    {
        _logger.LogInformation($"Received the message '{message}' from the queue.");
    }
}
#endif
