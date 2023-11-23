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
            packages.Add(("Steeltoe.Messaging.RabbitMQ", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Config");
            snippets.Add("Steeltoe.Messaging.RabbitMQ.Extensions");

            snippets.Add("builder.Services.AddRabbitServices(true)");
            snippets.Add("builder.Services.AddRabbitAdmin()");
            snippets.Add("builder.Services.AddRabbitQueue(");
            snippets.Add("builder.Services.AddSingleton<RabbitListenerService>()");
            snippets.Add("builder.Services.AddRabbitListeners<RabbitListenerService>()");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting RabbitListenerService");
            Sandbox.FileExists(GetSourceFileForLanguage("Services/RabbitListenerService", options.Language)).Should().BeTrue();
        }
    }
}
