using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class RedisProjectOptionTest : ProjectOptionTest
    {
        public RedisProjectOptionTest(ITestOutputHelper logger) : base("redis",
            "Add access to Redis data stores", logger)
        {
        }

        protected override void AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Microsoft.Extensions.Caching.StackExchangeRedis", "3.1.*"));
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            if (steeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("using Steeltoe.CloudFoundry.Connector.Redis;");
            }
            else
            {
                snippets.Add("using Steeltoe.Connector.Redis;");
            }

            snippets.Add("services.AddDistributedRedisCache(Configuration)");
        }

        protected override void AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion,
            Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Microsoft.Extensions.Caching.Distributed;");
            snippets.Add("using System.Threading.Tasks;");
            snippets.Add("using System.Collections.Generic;");
            snippets.Add(@"
public ValuesController(IDistributedCache cache)
{
    _cache = cache;
}
");
            snippets.Add(@"
[HttpGet]
public async Task<IEnumerable<string>> Get()
{
    await _cache.SetStringAsync(""MyValue1"", ""123"");
    await _cache.SetStringAsync(""MyValue2"", ""456"");
    string myval1 = await _cache.GetStringAsync(""MyValue1"");
    string myval2 = await _cache.GetStringAsync(""MyValue2"");
    return new[] { myval1, myval2};
}
");
        }
    }
}
