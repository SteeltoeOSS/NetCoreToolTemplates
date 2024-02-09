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
            "Add a RabbitMQ client controller for sending and receiving messages", logger)
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
            snippets.Add("builder.Services.AddRabbitTemplate()");
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Controllers/RabbitMessageController");
            Sandbox.FileExists(GetSourceFileForLanguage("Controllers/RabbitMessageController", options.Language))
                .Should().BeTrue();
        }
    }
}
