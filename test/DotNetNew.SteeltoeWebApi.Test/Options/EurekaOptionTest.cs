using System.Collections.Generic;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class EurekaOptionTest : OptionTest
    {
        public EurekaOptionTest(ITestOutputHelper logger) : base("eureka",
            "Add access to Eureka, a REST-based service for locating services", logger)
        {
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            packages.Add(("Steeltoe.Discovery.ClientCore", "$(SteeltoeVersion)"));
        }

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
        {
            snippets.Add("services.AddDiscoveryClient(Configuration);");
            snippets.Add("app.UseDiscoveryClient();");
        }
    }
}
