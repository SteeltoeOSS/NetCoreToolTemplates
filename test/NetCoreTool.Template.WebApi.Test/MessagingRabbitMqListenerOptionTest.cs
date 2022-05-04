using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MessagingRabbitMqListenerOptionTest : ProjectOptionTest
    {
        public MessagingRabbitMqListenerOptionTest(ITestOutputHelper logger) : base("messaging-rabbitmq-listener",
            "Add a RabbitMQ listener service for processing messages", logger)
        {
        }
    }
}
