using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MessagingRabbitMqClientOptionTest : ProjectOptionTest
    {
        public MessagingRabbitMqClientOptionTest(ITestOutputHelper logger) : base("messaging-rabbitmq-client",
            "Add a RabbitMQ client service for sending and receiving messages", logger)
        {
        }
    }
}
