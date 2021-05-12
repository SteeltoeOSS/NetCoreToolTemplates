using System.Threading.Tasks;
using Steeltoe.DotNetNew.Test.Utilities;
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

        protected async Task<Sandbox> TemplateSandbox(string args = "", bool exactly = false)
        {
            var command = $"dotnet new stwebapi";
            if (!string.IsNullOrEmpty(args))
            {
                command += $" {args}";
            }

            if (!exactly && !command.Contains("--help"))
            {
                command = command.Replace("stwebapi", "stwebapi --no-restore");
            }

            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command);
            return sandbox;
        }
    }
}
