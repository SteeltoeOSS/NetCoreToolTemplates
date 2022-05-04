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

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Config");
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Extensions");

            snippets.Add("services.AddRabbitServices(true)");
            snippets.Add("services.AddRabbitAdmin()");
            snippets.Add("services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE))");
            snippets.Add("services.AddSingleton<RabbitListenerService>()");
            snippets.Add("services.AddRabbitListeners<RabbitListenerService>()");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting RabbitListenerService");
            Sandbox.FileExists(GetSourceFileForLanguage("RabbitListenerService", options.Language)).Should().BeTrue();
        }
    }
}
