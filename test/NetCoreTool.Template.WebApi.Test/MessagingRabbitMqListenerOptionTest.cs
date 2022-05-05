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
            snippets.Add("services.AddRabbitQueue(");
            snippets.Add("services.AddSingleton<RabbitListenerService>()");
            snippets.Add("services.AddRabbitListeners<RabbitListenerService>()");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                return;
            }

            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting RabbitListenerService");
            Sandbox.FileExists(GetSourceFileForLanguage("Services/RabbitListenerService", options.Language)).Should().BeTrue();
        }
    }
}
