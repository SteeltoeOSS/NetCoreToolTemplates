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

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                return;
            }

            packages.Add(("Steeltoe.Messaging.RabbitMQ", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                return;
            }

            snippets.Add("Steeltoe.Messaging.RabbitMQ.Config");
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Extensions");

            snippets.Add("services.AddRabbitServices(true)");
            snippets.Add("services.AddRabbitAdmin()");
            snippets.Add("services.AddRabbitQueue(new Queue(ReceiveAndConvertQueue))");
            snippets.Add("services.AddRabbitTemplate()");
        }


        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                return;
            }

            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Controllers/WriteMessageController");
            Sandbox.FileExists(GetSourceFileForLanguage("Controllers/WriteMessageController", options.Language))
                .Should().BeTrue();
        }
    }
}
