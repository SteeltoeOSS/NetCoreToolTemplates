using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DynamicLoggerOptionTest : Test
    {
        public DynamicLoggerOptionTest(ITestOutputHelper logger) : base("dynamic-logger", logger)
        {
        }

        [Fact]
        public override async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            sandbox.CommandOutput.Should().ContainSnippet(@"
--dynamic-logger  Use dynamic logger.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestCsproj()
        {
            using var sandbox = await TemplateSandbox();
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var expectedPackageRefs = new List<string>
            {
                "Steeltoe.Extensions.Logging.DynamicLogger",
            };
            var actualPackageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e.Value
            ).ToList();
            actualPackageRefs.Should().Contain(expectedPackageRefs);
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Steeltoe.Extensions.Logging;");
            programSource.Should()
                .ContainSnippet(".ConfigureLogging((context, builder) => builder.AddDynamicConsole())");
        }

        [Fact]
        public async void TestProgramCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Steeltoe.Extensions.Logging;");
            programSource.Should()
                .ContainSnippet(
                    "loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(\"Logging\"));");
            programSource.Should().ContainSnippet(@"
.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(""Logging""));
    loggingBuilder.AddDynamicConsole();
 })
 ");
        }
    }
}
