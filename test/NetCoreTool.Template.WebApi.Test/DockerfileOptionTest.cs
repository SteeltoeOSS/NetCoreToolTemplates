using System;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DockerfileOptionTest : ProjectOptionTest
    {
        public DockerfileOptionTest(ITestOutputHelper logger) : base("dockerfile", "Add a Dockerfile", logger)
        {
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Logger.WriteLine("asserting Dockerfile");
            var dockerfile = await Sandbox.GetFileTextAsync("Dockerfile");
            var tag = options.Framework switch
            {
                Framework.Net50 => "5.0-alpine",
                Framework.NetCoreApp31 => "3.1-alpine",
                _ => throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString())
            };
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/aspnet:{tag} AS base");
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/sdk:{tag} AS build");
            var projectFile = GetProjectFileForLanguage(Sandbox.Name, options.Language);
            dockerfile.Should().ContainSnippet($"COPY [\"{projectFile}\", \".\"]");
            dockerfile.Should().ContainSnippet($"RUN dotnet build \"{projectFile}\"");
            dockerfile.Should().ContainSnippet($"RUN dotnet publish \"{projectFile}\"");
            dockerfile.Should().ContainSnippet($"ENTRYPOINT [\"dotnet\", \"{Sandbox.Name}.dll\"");
        }
    }
}
