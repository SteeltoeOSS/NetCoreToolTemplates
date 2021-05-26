using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class AzureSpringCloudOptionTest : OptionTest
    {
        public AzureSpringCloudOptionTest(ITestOutputHelper logger) : base("azure-spring-cloud", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--azure-spring-cloud  Add hosting support for Microsoft Azure Spring Cloud.
                      bool - Optional
                      Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    packageRefs.Should().NotContain("Microsoft.Azure.SpringCloud.Client");
                    break;
                default:
                    packageRefs.Should().Contain("Microsoft.Azure.SpringCloud.Client");
                    break;
            }
        }

        protected override void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertProgramCs(steeltoe, framework, source);
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    source.Should().NotContainSnippet("using Microsoft.Azure.SpringCloud.Client;");
                    break;
                default:
                    source.Should().ContainSnippet("using Microsoft.Azure.SpringCloud.Client;");
                    source.Should().ContainSnippet(".UseAzureSpringCloudService()");
                    break;
            }
        }
    }
}
