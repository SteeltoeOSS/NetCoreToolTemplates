using System.Text;
using System.Threading.Tasks;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public abstract class OptionTest : TemplateTest
    {
        protected OptionTest(string option, string description, ITestOutputHelper logger) : base(option, description,
            logger)
        {
        }

        protected override string GetSmokeTestArgs()
        {
            return "";
        }

        protected override async Task<Sandbox> TemplateSandbox(string args = "")
        {
            var normalizedArgs = new StringBuilder();
            if (Option is not null && !args.Contains("--help"))
            {
                normalizedArgs.Append("--").Append(Option).Append(' ');
            }

            normalizedArgs.Append(args);
            return await base.TemplateSandbox(normalizedArgs.ToString());
        }
    }
}
