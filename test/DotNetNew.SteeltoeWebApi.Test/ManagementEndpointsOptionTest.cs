using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class ManagementEndpointsOptionTest : OptionTest
    {
        public ManagementEndpointsOptionTest(ITestOutputHelper logger) : base("management-endpoints", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--management-endpoints  Add application management endpoints, such as health and metrics.
                        bool - Optional
                        Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    packageRefs.Should().Contain("Steeltoe.Management.CloudFoundryCore");
                    break;
                default:
                    packageRefs.Should().Contain("Steeltoe.Management.EndpointCore");
                    break;
            }
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("using Steeltoe.Management.CloudFoundry;");
                    source.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
                    source.Should().ContainSnippet("app.UseCloudFoundryActuators()");
                    break;
                default:
                    source.Should().ContainSnippet("using Steeltoe.Management.Endpoint;");
                    source.Should().ContainSnippet("services.AddAllActuators(Configuration);");
                    break;
            }
        }
    }
}
