using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConfigurationSpringBootOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("configuration-spring-boot", "Add support for reading Spring Boot-style keys from configuration.", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        packages.Add((GetPackageName(options.SteeltoeVersion), "$(SteeltoeVersion)"));
    }

    private static string GetPackageName(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.SpringBootCore" : "Steeltoe.Configuration.SpringBoot";
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        snippets.Add($"using {GetNamespaceImport(options.SteeltoeVersion)};");
        snippets.AddRange(GetSetupCodeFragments(options.SteeltoeVersion));
    }

    private static string GetNamespaceImport(SteeltoeVersion steeltoeVersion)
    {
        return steeltoeVersion == SteeltoeVersion.Steeltoe32 ? "Steeltoe.Extensions.Configuration.SpringBoot" : "Steeltoe.Configuration.SpringBoot";
    }

    private static IEnumerable<string> GetSetupCodeFragments(SteeltoeVersion steeltoeVersion)
    {
        if (steeltoeVersion == SteeltoeVersion.Steeltoe32)
        {
            yield return "builder.Configuration.AddSpringBootEnv();";
            yield return "builder.Configuration.AddSpringBootCmd(builder.Configuration);";
        }
        else
        {
            yield return "builder.Configuration.AddSpringBootFromEnvironmentVariable();";
            yield return "builder.Configuration.AddSpringBootFromCommandLine(args);";
        }
    }
}
