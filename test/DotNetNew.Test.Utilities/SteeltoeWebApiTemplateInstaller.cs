using System.Diagnostics;
using System.IO;
using Xunit;
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
                var p = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"new -i {Directory.GetCurrentDirectory()}/../../../../../src/DotNetNew.WebApi",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                );
                Assert.NotNull(p);
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
                Assert.True(p.ExitCode == 0);
            }

            internal static void EnsureInstalled(ITestOutputHelper logger)
            {
                logger.WriteLine("template stwebapi installed");
            }
        }
    }
}
