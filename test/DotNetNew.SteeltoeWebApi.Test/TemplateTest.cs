using System.Text;
using System.Threading.Tasks;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public abstract class TemplateTest
    {
        protected readonly ITestOutputHelper Logger;
        protected TemplateTest(ITestOutputHelper logger)
        {
            Logger = logger;
        }

        protected virtual async Task<Sandbox> TemplateSandbox(string args = "")
        {
            Assert.NotNull(args);
            var command = new StringBuilder("dotnet new steeltoe-webapi");

            if (args.Length > 1)
            {
                command.Append(' ').Append(args);
            }

            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command.ToString());
            return sandbox;
        }
    }
}
