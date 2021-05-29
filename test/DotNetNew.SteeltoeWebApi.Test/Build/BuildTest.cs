using System.Threading.Tasks;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Build
{
    public class BuildTest : TemplateTest
    {
        public BuildTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        [Trait("Category", "Build")]
        public async void TestDefault()
        {
            Logger.WriteLine($"testing default build");
            await BuildProject();
        }

        [Theory]
        [Trait("Category", "Build")]
        [ClassData(typeof(TemplateOptions.SteeltoeVersionsAndFrameworks))]
        public async void TestSteeltoeFramework(string steeltoe, string framework)
        {
            Logger.WriteLine($"testing {steeltoe}/{framework} build");
            await BuildProject($"--steeltoe {steeltoe} --framework {framework}");
        }

        [Theory]
        [Trait("Category", "Build")]
        [ClassData(typeof(TemplateOptions.SteeltoeVersionsAndFrameworksAndOptions))]
        public async void TestSteeltoeFrameworkOption(string steeltoe, string framework, string option)
        {
            Logger.WriteLine($"testing {steeltoe}/{framework}/{option} build");
            await BuildProject($"--steeltoe {steeltoe} --framework {framework} --{option}");
        }

        private async Task BuildProject(string args = "")
        {
            Logger.WriteLine("generating project");
            using var sandbox = await TemplateSandbox($"--no-restore {args}".Trim());
            Logger.WriteLine("restoring project");
            await sandbox.ExecuteCommandAsync("dotnet restore");
            Logger.WriteLine("building project");
            await sandbox.ExecuteCommandAsync("dotnet build /p:TreatWarningsAsErrors=True");
        }
    }
}
