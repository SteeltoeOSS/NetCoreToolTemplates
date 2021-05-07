using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
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
                     net5.0
                     netcoreapp3.1
                     netcoreapp2.1
                 Default: net5.0
");
        }

        [Fact]
        public async void TestFrameworkDefault()
        {
            using var sandbox = Sandbox();
            const string framework = "net5.0";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi");
            var expectedFrameworks = new List<string> { framework };
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var actualFrameworks =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("TargetFramework")
                select e.Value
            ).ToList();
            actualFrameworks.Should().BeEquivalentTo(expectedFrameworks);
        }

        [Fact]
        public async void TestFrameworkNet50()
        {
            using var sandbox = Sandbox();
            const string framework = "net5.0";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            // <project>.csproj
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
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
            startupSource.Should().ContainSnippet("using Microsoft.OpenApi.Models;");
            startupSource.Should().ContainSnippet("services.AddControllers();");
            startupSource.Should().ContainSnippet(@"
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""@@NAME@@"", Version = ""v1"" });
        });
".Replace("@@NAME@@", sandbox.Name));
            startupSource.Should().ContainSnippet(@"
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(""/swagger/v1/swagger.json"", ""@@NAME@@ v1""));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
".Replace("@@NAME@@", sandbox.Name));
            // Properties/launchSettings.json
            var launchSettings = await sandbox.GetJsonDocumentAsync<LaunchSettings>("Properties/launchSettings.json");
            launchSettings.Profiles["IIS Express"].LaunchUrl.Should().Be("swagger");
            launchSettings.Profiles[sandbox.Name].LaunchUrl.Should().Be("swagger");
        }

        [Fact]
        public async void TestFrameworkNetCoreApp31()
        {
            using var sandbox = Sandbox();
            const string framework = "netcoreapp3.1";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            // <project>.csproj
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
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
            // Properties/launchSettings.json
            var launchSettings = await sandbox.GetJsonDocumentAsync<LaunchSettings>("Properties/launchSettings.json");
            launchSettings.Profiles["IIS Express"].LaunchUrl.Should().Be("api/values");
            launchSettings.Profiles[sandbox.Name].LaunchUrl.Should().Be("api/values");
        }

        [Fact]
        public async void TestFrameworkNetCoreApp21()
        {
            using var sandbox = Sandbox();
            const string framework = "netcoreapp2.1";
            await sandbox.ExecuteCommandAsync($"dotnet new stwebapi --framework {framework}");
            // <project>.csproj
            var xDoc = await sandbox.GetXmlDocumentAsync($"{sandbox.Name}.csproj");
            var expectedFrameworks = new List<string> { framework };
            var actualFrameworks =
            (
                from e in xDoc.Elements().Elements("PropertyGroup").Elements("TargetFramework")
                select e.Value
            ).ToArray();
            actualFrameworks.Should().BeEquivalentTo(expectedFrameworks);
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
            // Properties/launchSettings.json
            var launchSettings = await sandbox.GetJsonDocumentAsync<LaunchSettings>("Properties/launchSettings.json");
            launchSettings.Profiles["IIS Express"].LaunchUrl.Should().Be("api/values");
            launchSettings.Profiles[sandbox.Name].LaunchUrl.Should().Be("api/values");
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
