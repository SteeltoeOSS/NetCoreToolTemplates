using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class LoggingDynamicLoggerOptionTest : ProjectOptionTest
    {
        public LoggingDynamicLoggerOptionTest(ITestOutputHelper logger) : base("logging-dynamic-logger",
            "Add a dynamic logger",
            logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Extensions.Logging.DynamicLogger", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("Steeltoe.Extensions.Logging");
            snippets.Add(".ConfigureLogging(");
            if (options.Framework < Framework.NetCoreApp31)
            {
                snippets.Add(
                    "loggingBuilder.AddConfiguration");
                snippets.Add(".AddDynamicConsole(");
            }
        }
    }
}
