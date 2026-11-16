using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConnectorSqlServerEfCoreOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("connector-sqlserver-efcore", "Add a connector for Microsoft SQL Server databases using Entity Framework Core.", logger)
{
    [Fact]
    [Trait("Category", "ProjectGeneration")]
    public async Task TestDefaultNotPolluted()
    {
        using var sandbox = await TemplateSandbox("false");
        sandbox.FileExists("AppDbContext.cs").Should().BeFalse();
    }

    protected override async Task AssertProjectGeneration(ProjectOptions options)
    {
        await base.AssertProjectGeneration(options);

        Logger.WriteLine("asserting Models");
        Sandbox.FileExists(GetSourceFileForLanguage("AppDbContext", options.Language)).Should().BeTrue();
    }

    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Microsoft.EntityFrameworkCore", GetPackageVersionForFramework(options.Framework)));
        packages.Add(("Microsoft.EntityFrameworkCore.SqlServer", GetPackageVersionForFramework(options.Framework)));
        packages.Add(("Steeltoe.Connectors.EntityFrameworkCore", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add($"using {Sandbox.Name};");
        snippets.Add("using Steeltoe.Connectors.SqlServer;");
        snippets.Add("using Steeltoe.Connectors.EntityFrameworkCore.SqlServer;");
        snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString");
        snippets.Add("builder.AddSqlServer();");
        snippets.Add("builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider));");
    }
}
