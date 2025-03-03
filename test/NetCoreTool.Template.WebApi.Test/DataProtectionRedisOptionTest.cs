using System.Collections.Generic;
using System.Linq;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class DataProtectionRedisOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("data-protection-redis", "Configure the ASP.NET data protection system to persist keys in a Redis database.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.AddRange(GetPackageNames(options.SteeltoeVersion).Select(packageName => (packageName, "$(SteeltoeVersion)")));
    }

    private static IEnumerable<string> GetPackageNames(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return "Steeltoe.Connector.ConnectorCore";
            yield return "Steeltoe.Security.DataProtection.RedisCore";
        }
        else
        {
            yield return "Steeltoe.Security.DataProtection.Redis";
        }
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.AddRange(GetNamespaceImports(options.SteeltoeVersion));
        snippets.Add(GetSetupComment(options.SteeltoeVersion));
        snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
    }

    private static IEnumerable<string> GetNamespaceImports(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return "using Microsoft.AspNetCore.DataProtection;";
            yield return "using Steeltoe.Connector.Redis;";
            yield return "using Steeltoe.Security.DataProtection;";
        }
        else
        {
            yield return "using Microsoft.AspNetCore.DataProtection;";
            yield return "using Steeltoe.Connectors.Redis;";
            yield return "using Steeltoe.Security.DataProtection.Redis;";
        }
    }

    private static string GetSetupComment(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32
            ? "// TODO: Add your connection string at configuration key: Redis:Client:ConnectionString"
            : "// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString";
    }

    private IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return "builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);";
        }
        else
        {
            yield return "builder.AddRedis();";
        }

        yield return $@"builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName(""{Sandbox.Name}"");";
    }
}
