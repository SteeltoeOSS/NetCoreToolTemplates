using System.IO;
using FluentAssertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public sealed class SteeltoeWebApiTemplateInstaller
    {
        private readonly ITestOutputHelper _logger;

        public SteeltoeWebApiTemplateInstaller(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        public void Install()
        {
            Installer.EnsureInstalled(_logger);
        }

        private class Installer
        {
            static Installer()
            {
                new Command().ExecuteAsync(
                        $"dotnet new -i {Directory.GetCurrentDirectory()}/../../../../../src/DotNetNew.WebApi")
                    .GetAwaiter()
                    .GetResult();
                var p = new Command().ExecuteAsync("dotnet new").GetAwaiter().GetResult();
                var templateListing = p.StandardOutput.ReadToEnd();
                templateListing.Should().MatchRegex("Steeltoe Web API.*stwebapi.*[C#].*Steeltoe/WebAPI/C#");
            }

            internal static void EnsureInstalled(ITestOutputHelper logger)
            {
                logger.WriteLine("template stwebapi installed");
            }
        }
    }
}
