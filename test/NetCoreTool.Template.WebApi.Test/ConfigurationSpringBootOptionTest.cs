using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConfigurationSpringBootOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("configuration-spring-boot", "Add support for reading Spring Boot-style keys from configuration.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add(("Steeltoe.Configuration.SpringBoot", "$(SteeltoeVersion)"));
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add("using Steeltoe.Configuration.SpringBoot;");
        snippets.Add("builder.Configuration.AddSpringBootFromEnvironmentVariable();");
        snippets.Add("builder.Configuration.AddSpringBootFromCommandLine(args);");
    }
}
