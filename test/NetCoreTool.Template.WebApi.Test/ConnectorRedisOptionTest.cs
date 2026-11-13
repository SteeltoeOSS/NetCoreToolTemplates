using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorRedisOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("connector-redis", "Add a connector for Redis data stores.", logger)
    {
        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("Microsoft.Extensions.Caching.StackExchangeRedis", GetPackageVersionForFramework(options.Framework)));
            packages.Add(("Steeltoe.Connectors", "$(SteeltoeVersion)"));
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            snippets.Add("using Steeltoe.Connectors.Redis;");
            snippets.Add("// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString");
            snippets.Add("builder.AddRedis();");
        }
    }
}
