using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public class Sandbox : TempDirectory
    {
        private readonly ITestOutputHelper _logger;

        public string Name
        {
            get => System.IO.Path.GetFileName(Path);
        }

        public Sandbox(ITestOutputHelper logger) : base($"DotNetNewTemplatesSandboxes/P_{Guid.NewGuid()}")
        {
            _logger = logger;
            _logger.WriteLine($"sandbox: {Name}");
        }

        public async Task<Process> ExecuteCommandAsync(string command)
        {
            _logger.WriteLine($"executing: {command}");
            return await new Command().ExecuteAsync(command, Path);
        }

        public string Join(string path)
        {
            return System.IO.Path.Join(Path, path);
        }

        public void FileShouldExist(string path)
        {
            File.Exists(Join(path)).Should().BeTrue();
        }

        public void FileShouldContainSnippet(string path, string snippet)
        {
            File.ReadAllText(Join(path)).Should().ContainSnippet(snippet);
        }
    }
}
