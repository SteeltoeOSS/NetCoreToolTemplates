using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Config");
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Extensions");

            snippets.Add("services.AddRabbitServices(true)");
            snippets.Add("services.AddRabbitAdmin()");
            snippets.Add("services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE))");
            snippets.Add("services.AddRabbitTemplate()");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Controllers/WriteMessageController");
            Sandbox.FileExists(GetSourceFileForLanguage("Controllers/WriteMessageController", options.Language)).Should().BeTrue();
        }
    }
}
