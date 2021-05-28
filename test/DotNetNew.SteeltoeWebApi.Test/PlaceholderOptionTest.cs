using System;
using System.Collections.Generic;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class PlaceholderOptionTest : OptionTest
    {
        public PlaceholderOptionTest(ITestOutputHelper logger) : base("placeholder",
            "Add a placeholder configuration source", logger)
        {
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            packages.Add("Steeltoe.Extensions.Configuration.PlaceholderCore");
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe2:
                    snippets.Add("using Steeltoe.Extensions.Configuration.PlaceholderCore;");
                    break;
                default:
                    snippets.Add("using Steeltoe.Extensions.Configuration.Placeholder;");
                    break;
            }

            snippets.Add(".AddPlaceholderResolver()");
        }

        protected override void AddValuesControllerCsSnippets(Steeltoe steeltoe, Framework framework,
            List<string> snippets)
        {
            snippets.Add("using Microsoft.Extensions.Configuration;");
            snippets.Add("private readonly IConfiguration _configuration;");
            snippets.Add(@"
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

        protected override void AddAppSettingsAssertions(List<Action<Steeltoe, Framework, AppSettings>> assertions)
        {
            assertions.Add(AssertPlaceholderSettings);
        }

        private void AssertPlaceholderSettings(Steeltoe steeltoe, Framework framework, AppSettings settings)
        {
            settings.ResolvedPlaceholderFromEnvVariables.Should().Be("${PATH?NotFound}");
            settings.ResolvedPlaceholderFromJson.Should()
                .Be("${Logging:LogLevel:System?${Logging:LogLevel:Default}}");
            settings.UnresolvedPlaceholder.Should().Be("${SomKeyNotFound?NotFound}");
        }
    }
}
