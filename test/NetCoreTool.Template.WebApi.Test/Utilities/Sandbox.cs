using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
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
            ReadCommentHandling = JsonCommentHandling.Skip
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

        public async Task ExecuteCommandAsync(string command, bool throwOnNonZeroExitCode)
        {
            _logger.WriteLine($"executing: {command}");
            (CommandExitCode, CommandOutput) = await new Command().ExecuteAsync(command, Path, throwOnNonZeroExitCode);
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

        private async Task<XDocument> GetXmlDocumentAsync(string path)
        {
            var fileText = await GetFileTextAsync(path);
            using var sin = new StringReader(fileText);
            return XDocument.Load(sin);
        }

        public async Task<ProjectFile> GetProjectFileAsync(string path)
        {
            ProjectFile projectFile = new();

            var document = await GetXmlDocumentAsync(path);
            var projectElement = document.Element("Project")!;

            foreach (var groupElement in projectElement.Elements("ItemGroup"))
            {
                ProjectItemGroup itemGroup = new();
                foreach (var packageElement in groupElement.Elements("PackageReference"))
                {
                    var include = packageElement.Attribute("Include")?.Value;
                    var condition = packageElement.Attribute("Condition")?.Value;
                    var version = packageElement.Attribute("Version")?.Value;

                    itemGroup.PackageReferences.Add(new PackageReference
                    {
                        Include = include,
                        Condition = condition,
                        Version = version
                    });
                }

                projectFile.ItemGroups.Add(itemGroup);
            }

            foreach (var groupElement in projectElement.Elements("PropertyGroup"))
            {
                ProjectPropertyGroup propertyGroup = new();
                foreach (var propertyElement in groupElement.Elements())
                {
                    var name = propertyElement.Name.ToString();
                    var value = propertyElement.Value;

                    propertyGroup.Properties.Add(new ProjectProperty { Name = name, Value = value });
                }

                projectFile.PropertyGroups.Add(propertyGroup);
            }

            return projectFile;
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
