using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class DataProtectionRedisOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("data-protection-redis", "Configure the ASP.NET data protection system to persist keys in a Redis database.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Steeltoe.Security.DataProtection.Redis", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Microsoft.AspNetCore.DataProtection;");
        snippets.Add("using Steeltoe.Connectors.Redis;");
        snippets.Add("using Steeltoe.Security.DataProtection.Redis;");
        snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString");
        snippets.Add("builder.AddRedis();");
        snippets.Add($@"builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName(""{Sandbox.Name}"");");
    }
}
