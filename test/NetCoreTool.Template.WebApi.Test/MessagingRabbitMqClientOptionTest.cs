using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MessagingRabbitMqClientOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("messaging-rabbitmq-client", "Add a RabbitMQ client service for sending and receiving messages (Steeltoe 3.x only).", logger)
    {
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
                snippets.Add("using Steeltoe.Messaging.RabbitMQ.Core;");

                snippets.Add("builder.Services.AddRabbitServices(true);");
                snippets.Add("builder.Services.AddRabbitAdmin();");
                snippets.Add(@"builder.Services.AddRabbitQueue(new Queue(""steeltoe_message_queue""));");
                snippets.Add("builder.Services.AddRabbitTemplate();");
                snippets.Add(@"app.MapGet(""/sendtoqueue"", ([FromServices] RabbitTemplate rabbitTemplate, [FromServices] RabbitAdmin rabbitAdmin) =>");
            }
        }
    }
}
