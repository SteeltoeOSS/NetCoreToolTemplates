using System.Text;
using System.Threading.Tasks;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public abstract class Test
    {
        protected readonly ITestOutputHelper Logger;

        public Test(ITestOutputHelper logger)
        {
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
        }

        protected async Task<Sandbox> TemplateSandbox(string args = "")
        {
            Assert.NotNull(args);
            var command = new StringBuilder("dotnet new stwebapi");
            if (!(args.Contains("--help") || args.Contains("--no-restore")))
            {
                command.Append(" --no-restore");
            }

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
