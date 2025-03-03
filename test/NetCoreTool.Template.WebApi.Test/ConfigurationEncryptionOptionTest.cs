using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public class ConfigurationEncryptionOptionTest(ITestOutputHelper logger)
    : ProjectOptionTest("configuration-encryption", "Add decryption of encrypted settings in configuration (Steeltoe 4.0 or higher).", logger)
{
    protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
    {
        if (options.SteeltoeVersion != SteeltoeVersion.Steeltoe32)
        {
            packages.Add(("Steeltoe.Configuration.Encryption", "$(SteeltoeVersion)"));
        }
    }

    protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
    {
        if (options.SteeltoeVersion != SteeltoeVersion.Steeltoe32)
        {
            snippets.Add("using Steeltoe.Configuration.Encryption;");

            snippets.Add("builder.Configuration.AddDecryption();");
        }
    }
}
