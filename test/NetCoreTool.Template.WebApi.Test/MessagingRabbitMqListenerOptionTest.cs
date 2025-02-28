using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MessagingRabbitMqListenerOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("messaging-rabbitmq-listener", "Add a RabbitMQ listener service for processing messages (Steeltoe 3.x only).", logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("RabbitListenerService.cs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                await base.AssertProjectGeneration(options);
                Logger.WriteLine("asserting Services");
                Sandbox.FileExists(GetSourceFileForLanguage("RabbitListenerService", options.Language)).Should().BeTrue();
            }
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                packages.Add(("Steeltoe.Messaging.RabbitMQ", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion == SteeltoeVersion.Steeltoe32)
            {
                snippets.Add("using Steeltoe.Messaging.RabbitMQ.Config;");
                snippets.Add("using Steeltoe.Messaging.RabbitMQ.Extensions;");
                snippets.Add($"using {Sandbox.Name};");

                snippets.Add("builder.Services.AddRabbitServices(true);");
                snippets.Add("builder.Services.AddRabbitAdmin();");
                snippets.Add(@"builder.Services.AddRabbitQueue(new Queue(""steeltoe_message_queue""));");
                snippets.Add("builder.Services.AddSingleton<RabbitListenerService>();");
                snippets.Add("builder.Services.AddRabbitListeners<RabbitListenerService>();");
            }
        }
    }
}
