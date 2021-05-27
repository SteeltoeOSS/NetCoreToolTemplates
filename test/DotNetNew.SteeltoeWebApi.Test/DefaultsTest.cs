using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Assertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DefaultsTest : OptionTest
    {
        public DefaultsTest(ITestOutputHelper logger) : base(null, logger)
        {
        }

        protected override void AssertHelp(string help)
        {
            base.AssertHelp(help);
            help.Should().ContainSnippet(@"
Steeltoe Web API (C#)
Author: VMware
");
            help.Should().ContainSnippet(@"
-s|--steeltoe  Set the Steeltoe version for the project.
                 3.0.2
                 2.5.3
               Default: 3.0.2
");
            help.Should().ContainSnippet(@"
-f|--framework  Set the target framework for the project.
                  net5.0
                  netcoreapp3.1
                  netcoreapp2.1
                Default: net5.0
");
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedSteeltoe()
        {
            using var sandbox = await TemplateSandbox("--steeltoe unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --steeltoe");
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestUnsupportedFramework()
        {
            using var sandbox = await TemplateSandbox("--framework unsupported1.0");
            sandbox.CommandError.Should().Contain("'unsupported1.0' is not a valid value for --framework");
        }

        protected override async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            await base.AssertProject(steeltoe, framework);
            Sandbox.FileExists("app.config").Should().BeTrue();
        }

        protected override void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            base.AssertCsproj(steeltoe, framework, properties, packageRefs);
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe3:
                    properties["SteeltoeVersion"].Should().StartWith("3.");
                    break;
                case Steeltoe.Steeltoe2:
                    properties["SteeltoeVersion"].Should().StartWith("2.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe.ToString());
            }

            switch (framework)
            {
                case Framework.Net50:
                    properties["TargetFramework"].Should().Be("net5.0");
                    break;
                case Framework.NetCoreApp31:
                    properties["TargetFramework"].Should().Be("netcoreapp3.1");
                    break;
                case Framework.NetCoreApp21:
                    properties["TargetFramework"].Should().Be("netcoreapp2.1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }

            var expectedPackageRefs = framework switch
            {
                Framework.Net50 => new List<string>
                {
                    "Swashbuckle.AspNetCore",
                },
                Framework.NetCoreApp31 => new List<string>(),
                Framework.NetCoreApp21 => new List<string>
                {
                    "Microsoft.AspNetCore",
                    "Microsoft.AspNetCore.CookiePolicy",
                    "Microsoft.AspNetCore.HttpsPolicy",
                    "Microsoft.AspNetCore.Mvc",
                    "Microsoft.AspNetCore.Session",
                    "Microsoft.AspNetCore.StaticFiles",
                },
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString())
            };
            packageRefs.Should().BeEquivalentTo(expectedPackageRefs);
        }

        protected override void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertProgramCs(steeltoe, framework, source);
            switch (framework)
            {
                case Framework.Net50:
                case Framework.NetCoreApp31:
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.Hosting;");
                    source.Should().ContainSnippet("CreateHostBuilder(args).Build().Run();");
                    source.Should().ContainSnippet(@"
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
");
                    break;
                case Framework.NetCoreApp21:
                    source.Should().ContainSnippet("using Microsoft.AspNetCore;");
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
                    source.Should().ContainSnippet("CreateWebHostBuilder(args).Build().Run();");
                    source.Should().ContainSnippet(@"
public static IWebHostBuilder CreateWebHostBuilder(string[] args)
{
    var builder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    return builder;
}
");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }

        protected override void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            base.AssertStartupCs(steeltoe, framework, source);
            switch (framework)
            {
                case Framework.Net50:
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Builder;");
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.DependencyInjection;");
                    source.Should().ContainSnippet("using Microsoft.OpenApi.Models;");
                    source.Should().ContainSnippet("services.AddControllers();");
                    source.Should().ContainSnippet(@"
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""@@NAME@@"", Version = ""v1"" });
});
".Replace("@@NAME@@", Sandbox.Name));
                    source.Should().ContainSnippet(@"
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
".Replace("@@NAME@@", Sandbox.Name));
                    break;
                case Framework.NetCoreApp31:
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Builder;");
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.DependencyInjection;");
                    source.Should().ContainSnippet("services.AddControllers();");
                    source.Should().ContainSnippet(@"
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
}
");
                    break;
                case Framework.NetCoreApp21:
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Builder;");
                    source.Should().ContainSnippet("using Microsoft.AspNetCore.Hosting;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.Configuration;");
                    source.Should().ContainSnippet("using Microsoft.Extensions.DependencyInjection;");
                    source.Should().NotContainSnippet("using Microsoft.Extensions.Hosting;");
                    source.Should().ContainSnippet("services.AddMvc();");
                    source.Should().ContainSnippet(@"
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseMvc();
}
");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }

        protected override void AssertLaunchSettingsJson(Steeltoe steeltoe, Framework framework,
            LaunchSettings settings)
        {
            base.AssertLaunchSettingsJson(steeltoe, framework, settings);
            switch (framework)
            {
                case Framework.Net50:
                    settings.Profiles["IIS Express"].LaunchUrl.Should().Be("swagger");
                    settings.Profiles[Sandbox.Name].LaunchUrl.Should().Be("swagger");
                    break;
                case Framework.NetCoreApp31:
                case Framework.NetCoreApp21:
                    settings.Profiles["IIS Express"].LaunchUrl.Should().Be("api/values");
                    settings.Profiles[Sandbox.Name].LaunchUrl.Should().Be("api/values");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }
    }
}
