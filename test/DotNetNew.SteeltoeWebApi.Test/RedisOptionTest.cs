using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class RedisOptionTest : OptionTest
    {
        public RedisOptionTest(ITestOutputHelper logger) : base("redis", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet("--redis  Add access to Redis, an in-memory data structure store.");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            var expectedPackageRefs = new List<string>
            {
                "Microsoft.Extensions.Caching.StackExchangeRedis",
            };
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    expectedPackageRefs.Add("Steeltoe.CloudFoundry.ConnectorCore");
                    break;
                default:
                    expectedPackageRefs.Add("Steeltoe.Connector.ConnectorCore");
                    break;
            }
            packageRefs.Should().Contain(expectedPackageRefs);
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("using Steeltoe.CloudFoundry.Connector.Redis;");
                    break;
                default:
                    source.Should().ContainSnippet("using Steeltoe.Connector.Redis;");
                    break;
            }

            source.Should().ContainSnippet("services.AddDistributedRedisCache(Configuration)");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Microsoft.Extensions.Caching.Distributed;");
            source.Should().ContainSnippet("using System.Threading.Tasks;");
            source.Should().ContainSnippet("using System.Collections.Generic;");
            source.Should().ContainSnippet(@"
public ValuesController(IDistributedCache cache)
{
    _cache = cache;
}
");
            source.Should().ContainSnippet(@"
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
