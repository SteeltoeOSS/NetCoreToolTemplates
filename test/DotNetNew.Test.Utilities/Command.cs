using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public class Command
    {
        public async Task<Process> ExecuteAsync(string command, string workingDirectory = null)
        {
            var arguments = command.Split(" ", 2);
            var pinfo = new ProcessStartInfo
            {
                FileName = arguments[0],
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            if (arguments.Length == 2)
            {
                pinfo.Arguments = arguments[1];
            }

            if (workingDirectory != null)
            {
                pinfo.WorkingDirectory = workingDirectory;
            }

            var p = Process.Start(pinfo);
            Assert.NotNull(p);
            await p.WaitForExitAsync();
            if (p.ExitCode != 0)
            {
                var msg = new StringBuilder();
                msg.Append(await p.StandardOutput.ReadToEndAsync());
                msg.Append(await p.StandardError.ReadToEndAsync());
                throw new Exception(
                    $"'{p.StartInfo.FileName} {p.StartInfo.Arguments}' exit code {p.ExitCode} != 0\n\n{msg}");
            }

            return p;
        }
    }
}
