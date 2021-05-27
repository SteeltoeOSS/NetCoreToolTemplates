using System.Collections.Generic;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class EurekaOptionTest : OptionTest
    {
        public EurekaOptionTest(ITestOutputHelper logger) : base("eureka",
            "Add access to Eureka, a REST-based service for locating services", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.Discovery.ClientCore");
        }

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            snippets.Add("services.AddDiscoveryClient(Configuration);");
            snippets.Add("app.UseDiscoveryClient();");
        }
    }
}
