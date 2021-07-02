using System;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.Test.Utilities.Assertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Utils;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DockerfileOptionTest : ProjectOptionTest
    {
        public DockerfileOptionTest(ITestOutputHelper logger) : base("dockerfile", "Add a Dockerfile", logger)
        {
        }

        protected override async Task AssertProjectGeneration(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            await base.AssertProjectGeneration(steeltoeVersion, framework);
            Logger.WriteLine("asserting Dockerfile");
            var dockerfile = await Sandbox.GetFileTextAsync("Dockerfile");
            var tag = framework switch
            {
                Framework.Net50 => "5.0-alpine",
                Framework.NetCoreApp31 => "3.1-alpine",
                Framework.NetCoreApp21 => "2.1-alpine",
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString())
            };
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/aspnet:{tag} AS base");
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/sdk:{tag} AS build");
        }
    }
}
