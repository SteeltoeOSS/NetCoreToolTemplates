using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DynamicLoggerOptionTest : OptionTest
    {
        public DynamicLoggerOptionTest(ITestOutputHelper logger) : base("dynamic-logger", "Add a dynamic logger",
            logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.Extensions.Logging.DynamicLogger");
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
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
