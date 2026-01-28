using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test;

public sealed class AllOptionsTest(ITestOutputHelper logger) : ProjectOptionTest(null, "Author: Broadcom", logger)
{
    private static readonly Regex OptionNameRegex = new("(  |, )--(?<optionName>[a-z-]+)\\s", RegexOptions.Compiled);
    private static string[] _allOptions;

    private async Task<string[]> DiscoverOptionsAsync()
    {
        using var sandbox = new Sandbox(Logger);
        var command = "dotnet new steeltoe-webapi --help";
        await sandbox.ExecuteCommandAsync(command, false);
        sandbox.CommandExitCode.Should().Be(0, $"listing options should succeed, while output was:{Environment.NewLine}{sandbox.CommandOutput}");
        var templateOptionsText = sandbox.CommandOutput.Substring(sandbox.CommandOutput.IndexOf("Template options:", StringComparison.Ordinal));

        List<string> options = [];
        foreach (Match match in OptionNameRegex.Matches(templateOptionsText))
        {
            var optionValue = match.Groups["optionName"].Value;

            if (optionValue is not ("steeltoe" or "framework" or "language" or "no-restore"))
            {
                options.Add(optionValue);
            }
        }

        Logger.WriteLine($"Detected available options: {string.Join(" ", options)}");
        return options.ToArray();
    }

    protected override async Task<Sandbox> TemplateSandbox(string args = "", bool throwOnNonZeroExitCode = true)
    {
        var allOptions = await GetAllOptionsAsync();
        var argsWithAllOptions = $"{args} {string.Join(' ', allOptions.Select(option => $"--{option}"))}";
        return await base.TemplateSandbox(argsWithAllOptions, throwOnNonZeroExitCode);
    }

    private async Task<string[]> GetAllOptionsAsync()
    {
        return _allOptions ??= await DiscoverOptionsAsync();
    }
}
