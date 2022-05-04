using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting RabbitListenerService");
            Sandbox.FileExists(GetSourceFileForLanguage("RabbitListenerService", options.Language)).Should().BeTrue();
        }
    }
}
