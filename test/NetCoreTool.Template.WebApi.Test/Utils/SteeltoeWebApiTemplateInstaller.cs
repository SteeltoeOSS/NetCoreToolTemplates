using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Utils
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
                        Arguments = $"new --uninstall {Directory.GetCurrentDirectory()}/../../../../../src/Content",
                    }
                );
                Assert.NotNull(p);
                p.WaitForExit();
                p = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"new --install {Directory.GetCurrentDirectory()}/../../../../../src/Content",
                    }
                );
                Assert.NotNull(p);
                p.WaitForExit();
                Assert.True(p.ExitCode == 0);
            }

            internal static void EnsureInstalled(ITestOutputHelper logger)
            {
                logger.WriteLine("template steeltoe-webapi installed");
            }
        }
    }
}
