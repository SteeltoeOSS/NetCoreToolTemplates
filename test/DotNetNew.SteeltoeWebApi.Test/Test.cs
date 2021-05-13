using System.Text;
using System.Threading.Tasks;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    [Trait("Category", "Unit")]
    public abstract class Test
    {
        private readonly string _option;

        private readonly ITestOutputHelper _logger;

        public Test(ITestOutputHelper logger) : this(null, logger)
        {
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void TestTemplate()
        {
            (await TemplateSandbox()).Dispose();
        }

        public Test(string option, ITestOutputHelper logger)
        {
            _option = option;
            _logger = logger;
            new SteeltoeWebApiTemplateInstaller(_logger).Install();
        }

        protected async Task<Sandbox> TemplateSandbox(string args = "")
        {
            Assert.NotNull(args);
            var command = new StringBuilder("dotnet new stwebapi");
            if (args.Contains("--help"))
            {
            }
            else
            {
                if (!args.Contains("--no-restore"))
                {
                    command.Append(" --no-restore");
                }

                if (_option is not null)
                {
                    command.Append(" --").Append(_option);
                }
            }

            if (args.Length > 1)
            {
                command.Append(' ').Append(args);
            }

            var sandbox = new Sandbox(_logger);
            await sandbox.ExecuteCommandAsync(command.ToString());
            return sandbox;
        }
    }
}
