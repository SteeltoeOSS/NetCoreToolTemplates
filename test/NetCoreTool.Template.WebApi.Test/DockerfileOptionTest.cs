using System;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DockerfileOptionTest(ITestOutputHelper logger)
        : ProjectOptionTest("dockerfile", "Add a Dockerfile", logger)
    {
        [Fact]
        [Trait("Category", "ProjectGeneration")]
        public async Task TestDefaultNotPolluted()
        {
            using var sandbox = await TemplateSandbox("false");
            sandbox.FileExists("Dockerfile").Should().BeFalse();
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);

            Logger.WriteLine("asserting Dockerfile");
            var dockerfile = await Sandbox.GetFileTextAsync("Dockerfile");
            var tag = GetTag(options.Framework);
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/aspnet:{tag} AS base");
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/sdk:{tag} AS build");
            var projectFile = GetProjectFileForLanguage(Sandbox.Name, options.Language);
            dockerfile.Should().ContainSnippet($"COPY [\"{projectFile}\", \".\"]");
            dockerfile.Should().ContainSnippet($"RUN dotnet build \"./{projectFile}\"");
            dockerfile.Should().ContainSnippet($"RUN dotnet publish \"./{projectFile}\"");
            dockerfile.Should().ContainSnippet($"ENTRYPOINT [\"dotnet\", \"{Sandbox.Name}.dll\"");
        }

        private static string GetTag(Framework framework)
        {
            return framework switch
            {
                Framework.Net60 => "6.0",
                Framework.Net80 => "8.0",
                Framework.Net90 => "9.0",
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString())
            };
        }
    }
}
