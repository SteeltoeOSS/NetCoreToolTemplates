using Steeltoe.DotNetNew.Test.Utilities;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public abstract class SteeltoeWebApiTest
    {
        protected readonly ITestOutputHelper Logger;

        public SteeltoeWebApiTest(ITestOutputHelper logger)
        {
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
        }
    }
}
