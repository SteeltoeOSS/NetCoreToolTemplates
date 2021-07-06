using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MessagingRabbitMqOptionTest : ProjectOptionTest
    {
        public MessagingRabbitMqOptionTest(ITestOutputHelper logger) : base("messaging-rabbitmq",
            "Add RabbitMQ messaging support and auto-configuration", logger)
        {
        }

        protected override async Task AssertProjectGeneration(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            await base.AssertProjectGeneration(steeltoeVersion, framework);
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                Logger.WriteLine("asserting no Models/Message.cs");
                Sandbox.FileExists("Models/Message.cs").Should().BeFalse();
                Logger.WriteLine("asserting no Services/RabbitMessagesListener.cs");
                Sandbox.FileExists("Services/RabbitMessagesListener.cs").Should().BeFalse();
            }
            else
            {
                Logger.WriteLine("asserting Models/Message.cs");
                var msgCs = await Sandbox.GetFileTextAsync("Models/Message.cs");
                msgCs.Should().Contain("public class Message");
                Logger.WriteLine("asserting Services/RabbitMessageListener.cs");
                var servicesCs = await Sandbox.GetFileTextAsync("Services/RabbitMessagesListener.cs");
                servicesCs.Should().Contain("public RabbitMessagesListener(ILogger<RabbitMessagesListener> logger)");
            }
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                return;
            }

            packages.Add(("Steeltoe.Messaging.RabbitMQ", "$(SteeltoeVersion)"));
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                return;
            }

            snippets.Add("using Steeltoe.Messaging.RabbitMQ.Config;");
            snippets.Add("using Steeltoe.Messaging.RabbitMQ.Extensions;");
            snippets.Add("services.AddRabbitQueue(new Queue(\"my.message.queue\"));");
            snippets.Add("services.AddSingleton<RabbitMessagesListener>();");
            snippets.Add("services.AddRabbitListeners<RabbitMessagesListener>();");
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                return;
            }

            snippets.Add("using Steeltoe.Messaging.RabbitMQ.Host;");
            snippets.Add("RabbitHost.CreateDefaultBuilder()");
        }
    }
}
