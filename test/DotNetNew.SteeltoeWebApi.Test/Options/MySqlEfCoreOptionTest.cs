using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class MySqlEfCoreOptionTest : OptionTest
    {
        public MySqlEfCoreOptionTest(ITestOutputHelper logger) : base("mysql-efcore",
            "Add access to MySQL databases using Entity Framework Core", logger)
        {
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeFalse();
            sandbox.FileExists("Models/SampleContext.cs").Should().BeFalse();
        }

        protected override async Task AssertProject(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            await base.AssertProject(steeltoeVersion, framework);
            Logger.WriteLine("asserting Models/SampleContext.cs");
            var source = await Sandbox.GetFileTextAsync("Models/SampleContext.cs");
            source.Should().ContainSnippet("public class SampleContext : DbContext");
            Sandbox.FileExists("Models/ErrorViewModel.cs").Should().BeTrue();
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> packages)
        {
            packages.Add("Microsoft.EntityFrameworkCore");
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.Connector.EFCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.EFCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.MySql.EFCore;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.MySql.EFCore;");
                    break;
            }

            snippets.Add($"using {Sandbox.Name}.Models;");
            snippets.Add("services.AddDbContext<SampleContext>(options => options.UseMySql(Configuration));");
        }
    }
}
