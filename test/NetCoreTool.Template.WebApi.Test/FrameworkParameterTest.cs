using System.Linq;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class FrameworkParameterTest : ChoiceParameterTest
    {
        public FrameworkParameterTest(ITestOutputHelper logger) : base("framework", "The target framework for the project.", logger)
        {
            foreach (var framework in new TemplateOptions.BaseOptions().Select(parameter => parameter[1].ToString()).Distinct())
            {
                Values.Add(framework);
            }
        }
    }
}
