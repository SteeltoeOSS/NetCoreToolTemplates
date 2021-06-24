using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Steeltoe.NetCoreTool.Template.Test.Utilities
{
    public class Command
    {
        public async Task<string> ExecuteAsync(string command, string workingDirectory)
        {
            using var process = new Process();

            var arguments = command.Split(" ", 2);
            process.StartInfo.FileName = arguments[0];
            if (arguments.Length > 1)
            {
                process.StartInfo.Arguments = arguments[1];
            }

            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            var outputBuilder = new StringBuilder();
            var outputCloseEvent = new TaskCompletionSource<bool>();
            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data is null)
                {
                    outputCloseEvent.SetResult(true);
                }
                else
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            var errorBuilder = new StringBuilder();
            var errorCloseEvent = new TaskCompletionSource<bool>();
            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data is null)
                {
                    errorCloseEvent.SetResult(true);
                }
                else
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            try
            {
                if (!process.Start())
                {
                    throw new Exception($"'{command}' failed to start; no details available");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"'{command}' failed to start: {e.Message}");
            }

            const int timeoutMillis = 100 /* 100s */ * 1000 /* 1000ms/s */;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            var waitForExit = Task.Run(() => process.WaitForExit(timeoutMillis));
            var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);
            if (await Task.WhenAny(Task.Delay(timeoutMillis), processTask) == processTask && waitForExit.Result)
            {
                var output = $"{outputBuilder}${errorBuilder}";
                // if (process.ExitCode != 0)
                // {
                    // throw new Exception($"'{command}' exited with exit code {process.ExitCode}:\n\n{output}");
                // }

                return output;
            }

            try
            {
                process.Kill();
            }
            catch
            {
                // ignored
            }

            throw new Exception($"'{process.StartInfo.FileName} {process.StartInfo.Arguments}' timed out");
        }

        public struct CommandResult
        {
            public int ExitCode;
            public string Output;
        }
    }
}
