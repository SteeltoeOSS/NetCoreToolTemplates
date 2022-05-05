namespace Company.WebApplication.FS.Services
#if (MessagingRabbitMqListener)

open Microsoft.Extensions.Logging
open Steeltoe.Messaging.RabbitMQ.Attributes

type RabbitListenerService (logger : ILogger<RabbitListenerService>) =
    let logger = logger

    [<RabbitListener("steeltoe_message_queue")>]
    member _.ListenForAMessage(message : string) =
        logger.LogInformation(message)
#endif
