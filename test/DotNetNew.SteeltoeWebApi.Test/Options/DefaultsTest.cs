using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Options
{
    public class DefaultsTest : OptionTest
    {
        public DefaultsTest(ITestOutputHelper logger) : base(null, "Steeltoe Web API (C#) Author: VMware", logger)
        {
        }

        protected override async Task AssertProject(SteeltoeVersion steeltoeVersion, Framework framework)
        {
            await base.AssertProject(steeltoeVersion, framework);
            Sandbox.FileExists("app.config").Should().BeTrue();
        }

        protected override void AddProjectPackages(SteeltoeVersion steeltoeVersion, Framework framework,
            List<(string, string)> packages)
        {
            switch (framework)
            {
                case Framework.Net50:
                    packages.Add(("Swashbuckle.AspNetCore", "5.6.*"));
                    break;
                case Framework.NetCoreApp31:
                    break;
                case Framework.NetCoreApp21:
                    packages.Add(("Microsoft.AspNetCore", "$(NetCoreApp21Version)"));
                    packages.Add(("Microsoft.AspNetCore.CookiePolicy", "$(NetCoreApp21Version)"));
                    packages.Add(("Microsoft.AspNetCore.HttpsPolicy", "$(NetCoreApp21Version)"));
                    packages.Add(("Microsoft.AspNetCore.Mvc", "$(NetCoreApp21Version)"));
                    packages.Add(("Microsoft.AspNetCore.Session", "$(NetCoreApp21Version)"));
                    packages.Add(("Microsoft.AspNetCore.StaticFiles", "$(NetCoreApp21Version)"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(framework), framework.ToString());
            }
        }

        protected override void AddProjectProperties(SteeltoeVersion steeltoeVersion, Framework framework,
            Dictionary<string, string> properties)
        {
            switch (steeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe3:
                    properties["SteeltoeVersion"] = "3.0.*";
                    break;
                case SteeltoeVersion.Steeltoe2:
                    properties["SteeltoeVersion"] = "2.5.*";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(steeltoeVersion), steeltoeVersion.ToString());
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

        protected override void AddProgramCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
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

        protected override void AddStartupCsSnippets(SteeltoeVersion steeltoeVersion, Framework framework,
            List<string> snippets)
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

        protected override void AddLaunchSettingsAssertions(
            List<Action<SteeltoeVersion, Framework, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(SteeltoeVersion steeltoeVersion, Framework framework, LaunchSettings settings)
        {
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
