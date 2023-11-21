using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DescriptionParameterTest : ParameterTest
    {
        public DescriptionParameterTest(ITestOutputHelper logger) : base("description",
            "<description> Add a project description",
            logger)
        {
            Values.Add("dummy");
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task Description_Can_Be_Configured()
        {
            using var sandbox = await TemplateSandbox("\"my project\"");
            var project = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var description =
            (
                from e in project.Elements().Elements("PropertyGroup").Elements("Description")
                select (e.Value)
            ).ToList();
            description.Count.Should().Be(1);
            description[0].Should().Be("my project");
        }
    }
}
