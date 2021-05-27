using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class DefaultsTest : OptionTest
    {
        public DefaultsTest(ITestOutputHelper logger) : base(null, "Steeltoe Web API (C#) Author: VMware", logger)
        {
        }

        protected override async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            await base.AssertProject(steeltoe, framework);
            Sandbox.FileExists("app.config").Should().BeTrue();
        }

        protected override void AddProjectPackages(Steeltoe steeltoe, Framework framework, List<string> packages)
        {
            switch (framework)
            {
                case Framework.Net50:
                    packages.Add("Swashbuckle.AspNetCore");
                    break;
                case Framework.NetCoreApp31:
                    break;
                case Framework.NetCoreApp21:
                    packages.Add("Microsoft.AspNetCore");
                    packages.Add("Microsoft.AspNetCore.CookiePolicy");
                    packages.Add("Microsoft.AspNetCore.HttpsPolicy");
                    packages.Add("Microsoft.AspNetCore.Mvc");
                    packages.Add("Microsoft.AspNetCore.Session");
                    packages.Add("Microsoft.AspNetCore.StaticFiles");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }

        protected override void AddProjectProperties(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties)
        {
            switch (steeltoe)
            {
                case Steeltoe.Steeltoe3:
                    properties["SteeltoeVersion"] = "3.0.2";
                    break;
                case Steeltoe.Steeltoe2:
                    properties["SteeltoeVersion"] = "2.5.3";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe.ToString());
            }

            switch (framework)
            {
                case Framework.Net50:
                    properties["TargetFramework"] = "net5.0";
                    break;
                case Framework.NetCoreApp31:
                    properties["TargetFramework"] = "netcoreapp3.1";
                    break;
                case Framework.NetCoreApp21:
                    properties["TargetFramework"] = "netcoreapp2.1";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }

        protected override void AddProgramCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (framework)
            {
                case Framework.Net50:
                case Framework.NetCoreApp31:
                    snippets.Add("using Microsoft.AspNetCore.Hosting;");
                    snippets.Add("using Microsoft.Extensions.Hosting;");
                    snippets.Add("CreateHostBuilder(args).Build().Run();");
                    snippets.Add(@"
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
");
                    break;
                case Framework.NetCoreApp21:
                    snippets.Add("using Microsoft.AspNetCore;");
                    snippets.Add("using Microsoft.AspNetCore.Hosting;");
                    snippets.Add("CreateWebHostBuilder(args).Build().Run();");
                    snippets.Add(@"
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

        protected override void AddStartupCsSnippets(Steeltoe steeltoe, Framework framework, List<string> snippets)
        {
            switch (framework)
            {
                case Framework.Net50:
                    snippets.Add("using Microsoft.AspNetCore.Builder;");
                    snippets.Add("using Microsoft.AspNetCore.Hosting;");
                    snippets.Add("using Microsoft.Extensions.Configuration;");
                    snippets.Add("using Microsoft.Extensions.DependencyInjection;");
                    snippets.Add("using Microsoft.OpenApi.Models;");
                    snippets.Add("services.AddControllers();");
                    snippets.Add(@"
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""@@NAME@@"", Version = ""v1"" });
});
".Replace("@@NAME@@", Sandbox.Name));
                    snippets.Add(@"
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
                    snippets.Add("using Microsoft.AspNetCore.Builder;");
                    snippets.Add("using Microsoft.AspNetCore.Hosting;");
                    snippets.Add("using Microsoft.Extensions.Configuration;");
                    snippets.Add("using Microsoft.Extensions.DependencyInjection;");
                    snippets.Add("services.AddControllers();");
                    snippets.Add(@"
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
                    snippets.Add("using Microsoft.AspNetCore.Builder;");
                    snippets.Add("using Microsoft.AspNetCore.Hosting;");
                    snippets.Add("using Microsoft.Extensions.Configuration;");
                    snippets.Add("using Microsoft.Extensions.DependencyInjection;");
                    snippets.Add("services.AddMvc();");
                    snippets.Add(@"
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
