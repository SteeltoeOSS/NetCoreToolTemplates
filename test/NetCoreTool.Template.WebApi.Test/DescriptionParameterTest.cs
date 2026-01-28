using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DescriptionParameterTest : ParameterTest
    {
        public DescriptionParameterTest(ITestOutputHelper logger) : base("description", "Add a project description.",
            logger)
        {
            Values.Add("dummy");
        }

        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task Description_Can_Be_Configured()
        {
            using var sandbox = await TemplateSandbox("\"my project\"");
            var project = await sandbox.GetProjectFileAsync($"{sandbox.Name}.csproj");
            var properties = project.PropertyGroups.SelectMany(group => group.Properties).ToArray();

            properties.Should().ContainSingle(property => property.Name == "Description" && property.Value == "my project");
        }
    }
}
