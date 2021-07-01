using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class MySqlEfCoreOptionTest : ProjectOptionTest
    {
        public MySqlEfCoreOptionTest(ITestOutputHelper logger) : base("mysql-efcore",
            "Add access to MySQL databases using Entity Framework Core", logger)
        {
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async void TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeFalse();
            sandbox.FileExists("Models/SampleContext.cs").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            await base.AssertProjectGeneration(steeltoeVersion, framework);
            Logger.WriteLine("asserting Models/SampleContext.cs");
            var source = await Sandbox.GetFileTextAsync("Models/SampleContext.cs");
            source.Should().ContainSnippet("public class SampleContext : DbContext");
            Sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeTrue();
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Microsoft.EntityFrameworkCore", "3.1.*"));
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.Connector.EFCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.EFCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.CloudFoundry.Connector.MySql.EFCore;");
            }
            else
            {
                snippets.Add("using Steeltoe.Connector.MySql.EFCore;");
            }

            snippets.Add($"using {Sandbox.Name}.Models;");
            snippets.Add("services.AddDbContext<SampleContext>(options => options.UseMySql(Configuration));");
        }
    }
}
