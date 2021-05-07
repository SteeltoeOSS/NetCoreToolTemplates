using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public class Sandbox : TempDirectory
    {
        private readonly ITestOutputHelper _logger;

        public string CommandOutput { get; private set; }

        public string CommandError { get; private set; }

        public string Name
        {
            get => System.IO.Path.GetFileName(Path);
        }

        public Sandbox(ITestOutputHelper logger) : base($"DotNetNewTemplatesSandboxes/P_{Guid.NewGuid()}")
        {
            _logger = logger;
            _logger.WriteLine($"sandbox: {Name}");
        }

        public async Task ExecuteCommandAsync(string command)
        {
            _logger.WriteLine($"executing: {command}");
            var p = await new Command().ExecuteAsync(command, Path);
            CommandOutput = p.StandardOutput.ReadToEnd();
            CommandError = p.StandardError.ReadToEnd();
        }

        public bool FileExists(string path)
        {
            return File.Exists(Join(path));
        }

        public async Task<string> GetFileTextAsync(string path)
        {
            return await File.ReadAllTextAsync(Join(path));
        }

        public async Task<XDocument> GetXmlDocument(string path)
        {
            var fileText = await GetFileTextAsync(path);
            using var sin = new StringReader(fileText);
            return XDocument.Load(sin);
        }

        private string Join(string path)
        {
            return System.IO.Path.Join(Path, path);
        }
    }
}
