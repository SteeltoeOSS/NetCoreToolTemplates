using System;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class DockerOptionTest : Test
    {
        public DockerOptionTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
  --docker        Add Docker support.
                  bool - Optional
                  Default: false
");
        }

        [Fact]
        public async void TestDefault()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandExactlyAsync($"dotnet new stwebapi");
            sandbox.FileExists("Dockerfile").Should().BeFalse();
        }

        [Fact]
        public async void TestDockerfile()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandExactlyAsync($"dotnet new stwebapi --docker");
            sandbox.FileExists("Dockerfile").Should().Be(true);
        }

        [Theory]
        [InlineData("net5.0")]
        [InlineData("netcoreapp3.1")]
        [InlineData("netcoreapp2.1")]
        public async void TestFramework(string framework)
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --docker --framework {framework}");
            var dockerfile = await sandbox.GetFileTextAsync("Dockerfile");
            var tag = framework switch
            {
                "net5.0" => "5.0-alpine",
                "netcoreapp3.1" => "3.1-alpine",
                "netcoreapp2.1" => "2.1-alpine",
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework)
            };
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/aspnet:{tag} AS base");
            dockerfile.Should().ContainSnippet($"FROM mcr.microsoft.com/dotnet/sdk:{tag} AS build");
        }
    }
}
