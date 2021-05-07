using Steeltoe.DotNetNew.Test.Utilities;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public abstract class Test
    {
        protected readonly ITestOutputHelper Logger;

        public Test(ITestOutputHelper logger)
        {
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
        }

        protected Sandbox Sandbox()
        {
            return new(Logger);
        }
    }
}
