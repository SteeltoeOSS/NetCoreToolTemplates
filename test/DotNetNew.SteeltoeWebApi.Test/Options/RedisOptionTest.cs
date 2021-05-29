using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class RedisOptionTest : OptionTest
    {
        public RedisOptionTest(ITestOutputHelper logger) : base("redis",
            "Add access to Redis data stores", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Microsoft.Extensions.Caching.StackExchangeRedis", "3.1.*"));
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
                default:
                    packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
                    break;
            }
        }

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.Redis;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.Redis;");
                    break;
            }

            snippets.Add("services.AddDistributedRedisCache(Configuration)");
        }

        protected override void AddValuesControllerCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
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
