using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PlaceholderOptionTest : OptionTest
    {
        public PlaceholderOptionTest(ITestOutputHelper logger) : base("placeholder", logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
--placeholder  Add a placeholder configuration source.
               bool - Optional
               Default: false
");
        }

        protected override void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertProgramCs(steeltoe, framework, source);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.PlaceholderCore;");
                    break;
                default:
                    source.Should().ContainSnippet("using Steeltoe.Extensions.Configuration.Placeholder;");
                    break;
            }

            source.Should().ContainSnippet(".AddPlaceholderResolver()");
        }

        protected override void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertValuesControllerCs(steeltoe, framework, source);
            source.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            source.Should().ContainSnippet("private readonly IConfiguration _configuration;");
            source.Should().ContainSnippet(@"
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    var val1 = _configuration[""ResolvedPlaceholderFromEnvVariables""];
    var val2 = _configuration[""UnresolvedPlaceholder""];
    var val3 = _configuration[""ResolvedPlaceholderFromJson""];
    return new[] { val1, val2, val3 };
}
");
        }

        protected override void AssertAppSettingsJson(Steeltoe steeltoe, Framework framework, AppSettings settings)
        {
            base.AssertAppSettingsJson(steeltoe, framework, settings);
            settings.ResolvedPlaceholderFromEnvVariables.Should().Be("${PATH?NotFound}");
            settings.ResolvedPlaceholderFromJson.Should()
                .Be("${Logging:LogLevel:System?${Logging:LogLevel:Default}}");
            settings.UnresolvedPlaceholder.Should().Be("${SomKeyNotFound?NotFound}");
        }
    }
}
