using System.Text;
using System.Threading.Tasks;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public abstract class OptionTest(string option, string description, ITestOutputHelper logger)
        : TemplateTest(option, description, logger)
    {
        protected override string GetSmokeTestArgs()
        {
            return "";
        }

        protected override async Task<Sandbox> TemplateSandbox(string args = "", bool throwOnNonZeroExitCode = true)
        {
            var normalizedArgs = new StringBuilder();
            if (Option is not null && !args.Contains("--help"))
            {
                normalizedArgs.Append("--").Append(Option).Append(' ');
            }

            normalizedArgs.Append(args);
            return await base.TemplateSandbox(normalizedArgs.ToString(), throwOnNonZeroExitCode);
        }
    }
}
