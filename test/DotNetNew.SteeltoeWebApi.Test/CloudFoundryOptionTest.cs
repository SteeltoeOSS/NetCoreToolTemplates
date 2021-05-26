using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class CloudFoundryOptionTest : OptionTest

    {
        public CloudFoundryOptionTest(ITestOutputHelper logger) : base("cloud-foundry", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--cloud-foundry  Add hosting support for Cloud Foundry.
                 bool - Optional
                 Default: false
");
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            packageRefs.Should().Contain("Steeltoe.Common.Hosting");
            packageRefs.Should().Contain("Steeltoe.Extensions.Configuration.CloudFoundryCore");
        }

        protected override void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertProgramCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Steeltoe.Common.Hosting;");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            source.Should().ContainSnippet(".UseCloudHosting().AddCloudFoundryConfiguration()");
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            source.Should().ContainSnippet("services.ConfigureCloudFoundryOptions(Configuration);");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.CloudFoundry;");
            source.Should().ContainSnippet(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    string appName = _appOptions.ApplicationName;
    string appInstance = _appOptions.ApplicationId;
    return new[] { appInstance, appName };
}
");
        }
    }
}
