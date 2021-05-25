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
--dynamic-logger  Use a dynamic logger.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestProgramCs()
        {
            using var sandbox = await TemplateSandbox();
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Logging;");
            source.Should()
                .ContainSnippet(".ConfigureLogging((context, builder) => builder.AddDynamicConsole())");
        }

        [Fact]
        public async void TestProgramCsNetCoreApp21()
        {
            using var sandbox = await TemplateSandbox("--framework netcoreapp2.1");
            var source = await sandbox.GetFileTextAsync("Program.cs");
            source.Should().ContainSnippet("using Steeltoe.Extensions.Logging;");
            source.Should()
                .ContainSnippet(
                    "loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(\"Logging\"));");
            source.Should().ContainSnippet(@"
.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(""Logging""));
    loggingBuilder.AddDynamicConsole();
 })
 ");
        }
    }
}
