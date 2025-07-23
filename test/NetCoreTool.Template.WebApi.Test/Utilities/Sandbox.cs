using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities
{
    public class Sandbox : TempDirectory
    {
        private static JsonSerializerOptions Options { get; } = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        private readonly ITestOutputHelper _logger;

        public int CommandExitCode { get; private set; }

        public string CommandOutput { get; private set; }

        public string Name => System.IO.Path.GetFileName(Path);

        public Sandbox(ITestOutputHelper logger) : base(
            $"DotNetNewTemplatesSandboxes{System.IO.Path.DirectorySeparatorChar}P_{Guid.NewGuid():N}")
        {
            _logger = logger;
            _logger.WriteLine($"sandbox: {Name}");
        }

        public async Task ExecuteCommandAsync(string command)
        {
            _logger.WriteLine($"executing: {command}");
            (CommandExitCode, CommandOutput) = await new Command().ExecuteAsync(command, Path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(Join(path));
        }

        public bool FileExists(string path)
        {
            return File.Exists(Join(path));
        }

        public async Task<string> GetFileTextAsync(string path)
        {
            return await File.ReadAllTextAsync(Join(path));
        }

        public async Task<XDocument> GetXmlDocumentAsync(string path)
        {
            var fileText = await GetFileTextAsync(path);
            using var sin = new StringReader(fileText);
            return XDocument.Load(sin);
        }

        public async Task<T> GetJsonDocumentAsync<T>(string path)
        {
            var fileText = await GetFileTextAsync(path);
            return JsonSerializer.Deserialize<T>(fileText, Options);
        }

        private string Join(string path)
        {
            return System.IO.Path.Join(Path, path);
        }
    }
}
