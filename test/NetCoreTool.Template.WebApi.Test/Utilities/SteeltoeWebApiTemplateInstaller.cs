using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities
{
    public sealed class SteeltoeWebApiTemplateInstaller(ITestOutputHelper logger)
    {
        public void Install()
        {
            Installer.EnsureInstalled(logger);
        }

        private static class Installer
        {
            static Installer()
            {
                var process = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"new uninstall {Directory.GetCurrentDirectory()}/../../../../../src/Content"
                    }
                );
                Assert.NotNull(process);

                process.WaitForExit();
                process = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"new install {Directory.GetCurrentDirectory()}/../../../../../src/Content"
                    }
                );
                Assert.NotNull(process);

                process.WaitForExit();
                process.ExitCode.Should().Be(0);
            }

            internal static void EnsureInstalled(ITestOutputHelper logger)
            {
                logger.WriteLine("template steeltoe-webapi installed");
            }
        }
    }
}
