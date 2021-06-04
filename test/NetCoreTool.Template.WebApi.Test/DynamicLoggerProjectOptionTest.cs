using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DynamicLoggerProjectOptionTest : ProjectOptionTest
    {
        public DynamicLoggerProjectOptionTest(ITestOutputHelper logger) : base("dynamic-logger", "Add a dynamic logger",
            logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Logging.DynamicLogger", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Steeltoe.Extensions.Logging;");
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    snippets.Add(
                        "loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(\"Logging\"));");
                    snippets.Add(@"
.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(""Logging""));
    loggingBuilder.AddDynamicConsole();
 })
 ");
                    break;
                default:
                    snippets.Add(".ConfigureLogging((context, builder) => builder.AddDynamicConsole())");
                    break;
            }
        }
    }
}
