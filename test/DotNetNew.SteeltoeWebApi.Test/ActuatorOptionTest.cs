using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class ActuatorOptionTest : OptionTest
    {
        public ActuatorOptionTest(ITestOutputHelper logger) : base("actuator", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--actuator  Add endpoints to manage your application, such as health, metrics, etc.
            bool - Optional
            Default: false
");
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("Steeltoe.Management.CloudFoundry;");
                    source.Should().ContainSnippet("services.AddCloudFoundryActuators(Configuration);");
                    source.Should().ContainSnippet("app.UseCloudFoundryActuators()");
                    break;
                default:
                    source.Should().ContainSnippet("Steeltoe.Management.Endpoint;");
                    source.Should().ContainSnippet("services.AddAllActuators(Configuration);");
                    break;
            }
        }
    }
}
