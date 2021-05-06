using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class SteeltoeWebApiDefaultsTest : SteeltoeWebApiTest
    {
        public SteeltoeWebApiDefaultsTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestProject()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist($"{sandbox.Name}.csproj");
        }

        [Fact]
        public async void TestProgram()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldContainSnippet("Program.cs", "CreateHostBuilder(args).Build().Run();");
            sandbox.FileShouldContainSnippet("Program.cs",
                "Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults( webBuilder => { webBuilder.UseStartup<Startup>(); });");
        }

        [Fact]
        public async void TestStartup()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist("Startup.cs");
        }

        [Fact]
        public async void TestControllers()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist("Controllers/ValuesController.cs");
            sandbox.FileShouldContainSnippet("Controllers/ValuesController.cs",
                "public ActionResult<string> Get(int id) { return \"value\"; }");
        }

        [Fact]
        public async void TestProperties()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist("Properties/launchSettings.json");
        }

        [Fact]
        public async void TestSettings()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist("appsettings.json");
            sandbox.FileShouldExist("appsettings.Development.json");
        }

        [Fact]
        public async void TestConfig()
        {
            using var sandbox = new Sandbox(Logger);
            var p = await sandbox.ExecuteCommandAsync("dotnet new stwebapi");
            sandbox.FileShouldExist("app.config");
        }
    }
}
