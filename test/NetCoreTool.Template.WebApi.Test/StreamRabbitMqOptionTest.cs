using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class StreamRabbitMqOptionTest : ProjectOptionTest
    {
        public StreamRabbitMqOptionTest(ITestOutputHelper logger) : base("stream-rabbitmq",
            "Add RabbitMQ stream support and auto-configuration", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                return;
            }

            packages.Add(("Steeltoe.Stream.Binder.RabbitMQ", "$(SteeltoeVersion)"));
            packages.Add(("Steeltoe.Stream.StreamBase", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe31)
            {
                return;
            }

            snippets.Add("StreamHost.CreateDefaultBuilder<Program>(args)");
            snippets.Add("[StreamListener(ISink.INPUT)]");
            snippets.Add("[SendTo(ISource.OUTPUT)]");
        }
    }
}
