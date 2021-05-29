using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class DynamicLoggerOptionTest : OptionTest
    {
        public DynamicLoggerOptionTest(ITestOutputHelper logger) : base("dynamic-logger", "Add a dynamic logger",
            logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Logging.DynamicLogger", "$(SteeltoeVersion)"));
        }

        protected override void AddProgramCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
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
