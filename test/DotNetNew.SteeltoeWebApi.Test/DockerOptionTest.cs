using System;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DockerOptionTest : OptionTest
    {
        public DockerOptionTest(ITestOutputHelper logger) : base("docker", "Add support for Docker", logger)
        {
        }

        protected override async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            await base.AssertProject(steeltoe, framework);
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
