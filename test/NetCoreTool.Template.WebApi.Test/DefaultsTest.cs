using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class DefaultsTest : ProjectOptionTest
    {
        public DefaultsTest(ITestOutputHelper logger) : base(null, "Steeltoe Web API (C#) Author: VMware", logger)
        {
        }

        protected override async Task AssertProjectGeneration(ProjectOptions options)
        {
            await base.AssertProjectGeneration(options);
            Sandbox.FileExists("app.config").Should().BeTrue();
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            switch (options.Framework)
            {
                case Framework.Net50:
                    packages.Add(("Swashbuckle.AspNetCore", "5.6.*"));
                    break;
                case Framework.NetCoreApp31:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }

        protected override void AssertProjectPropertiesHook(ProjectOptions options, Dictionary<string, string> properties)
        {
            switch (options.SteeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe31:
                    properties["SteeltoeVersion"] = "3.1.*";
                    break;
                case SteeltoeVersion.Steeltoe30:
                    properties["SteeltoeVersion"] = "3.0.*";
                    break;
                case SteeltoeVersion.Steeltoe25:
                    properties["SteeltoeVersion"] = "2.5.*";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.SteeltoeVersion), options.SteeltoeVersion.ToString());
            }

            switch (options.Framework)
            {
                case Framework.Net50:
                    properties["TargetFramework"] = "net5.0";
                    break;
                case Framework.NetCoreApp31:
                    properties["TargetFramework"] = "netcoreapp3.1";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }

        protected override void AssertProgramSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            switch (options.Framework)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            switch (options.Framework)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }

        protected override void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(ProjectOptions options, LaunchSettings settings)
        {
            switch (options.Framework)
            {
                case Framework.Net50:
                    settings.Profiles["IIS Express"].LaunchUrl.Should().Be("swagger");
                    settings.Profiles[Sandbox.Name].LaunchUrl.Should().Be("swagger");
                    break;
                case Framework.NetCoreApp31:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }
    }
}
