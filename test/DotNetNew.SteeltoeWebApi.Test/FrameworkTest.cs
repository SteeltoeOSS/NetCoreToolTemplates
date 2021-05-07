using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.WebApi.Test
{
    public class FrameworkTest : Test
    {
        public FrameworkTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async void TestHelp()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync("dotnet new stwebapi -h");
            sandbox.CommandOutput.Should().ContainSnippet(@"
 -f|--framework  The target framework for the project.
                     netcoreapp3.1
                     netcoreapp2.1
                 Default: netcoreapp3.1
");
        }

        [Fact]
        public async void TestFrameworkNetCoreApp31()
        {
            using var sandbox = Sandbox();
            const string framework = "netcoreapp3.1";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            var xDoc = await sandbox.GetXmlDocument($"{sandbox.Name}.csproj");
            // frameworks
            var expectedFrameworks = new List<string> { framework };
            var actualFrameworks =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("TargetFramework")
                select e.Value
            ).ToList();
            actualFrameworks.Should().BeEquivalentTo(expectedFrameworks);
            // Program.cs
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
            programSource.Should().ContainSnippet("using Microsoft.Extensions.Hosting;");
            programSource.Should().ContainSnippet("CreateHostBuilder(args).Build().Run();");
            programSource.Should().ContainSnippet(@"
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
");
            // Startup.cs
            var startupSource = await sandbox.GetFileTextAsync("Startup.cs");
            startupSource.Should().ContainSnippet("using Microsoft.AspNetCore.Builder;");
            startupSource.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
            startupSource.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            startupSource.Should().ContainSnippet("using Microsoft.Extensions.DependencyInjection;");
            startupSource.Should().ContainSnippet("services.AddControllers();");
            startupSource.Should().ContainSnippet(@"
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
");
        }

        [Fact]
        public async void TestFrameworkNetCoreApp21()
        {
            using var sandbox = Sandbox();
            const string framework = "netcoreapp2.1";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            var xDoc = await sandbox.GetXmlDocument($"{sandbox.Name}.csproj");
            // frameworks
            var expectedFrameworks = new List<string> { framework };
            var actualFrameworks =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("TargetFramework")
                select e.Value
            ).ToArray();
            actualFrameworks.Should().BeEquivalentTo(expectedFrameworks);
            // package refs
            var expectedPackageRefs = new List<string>
            {
                "Microsoft.AspNetCore",
                "Microsoft.AspNetCore.CookiePolicy",
                "Microsoft.AspNetCore.HttpsPolicy",
                "Microsoft.AspNetCore.Mvc",
                "Microsoft.AspNetCore.Session",
                "Microsoft.AspNetCore.StaticFiles",
            };
            var actualPackageRefs =
            (
                from e in xDoc.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e
            ).ToList().Select(attr => attr.Value).ToList();
            actualPackageRefs.Should().BeEquivalentTo(expectedPackageRefs);
            // Program.cs
            var programSource = await sandbox.GetFileTextAsync("Program.cs");
            programSource.Should().ContainSnippet("using Microsoft.AspNetCore;");
            programSource.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
            programSource.Should().ContainSnippet("CreateWebHostBuilder(args).Build().Run();");
            programSource.Should().ContainSnippet(@"
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
            return builder;
        }
");
            // Startup.cs
            var startupSource = await sandbox.GetFileTextAsync("Startup.cs");
            startupSource.Should().ContainSnippet("using Microsoft.AspNetCore.Builder;");
            startupSource.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
            startupSource.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
            startupSource.Should().ContainSnippet("using Microsoft.Extensions.DependencyInjection;");
            startupSource.Should().NotContain("using Microsoft.Extensions.Hosting;");
            startupSource.Should().ContainSnippet("services.AddMvc();");
            startupSource.Should().ContainSnippet(@"
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseMvc();
        }
");
        }

        [Fact]
        public async void TestUnsupportedFramework()
        {
            using var sandbox = Sandbox();
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework netcoreapp2.0");
            sandbox.CommandError.Should().Contain("'netcoreapp2.0' is not a valid value");
        }
    }
}
