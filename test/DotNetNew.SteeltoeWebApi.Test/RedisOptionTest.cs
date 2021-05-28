using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RedisOptionTest : OptionTest
    {
        public RedisOptionTest(ITestOutputHelper logger) : base("redis",
            "Add access to Redis data stores", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Microsoft.Extensions.Caching.StackExchangeRedis");
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packages.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    packages.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    snippets.Add("using Steeltoe.CloudFoundry.Connector.Redis;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Connector.Redis;");
                    break;
            }

            snippets.Add("services.AddDistributedRedisCache(Configuration)");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
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
