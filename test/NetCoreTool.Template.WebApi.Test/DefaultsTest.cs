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
            if (options.Language == Language.CSharp)
            {
                Sandbox.FileExists("app.config").Should().BeTrue();
            }
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            if (options.Language == Language.CSharp)
            {
                switch (options.Framework)
                {
                    case Framework.Net60:
                    case Framework.Net50:
                        packages.Add(("Swashbuckle.AspNetCore", "6.2.*"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
                }
            }
        }

        protected override void AssertProjectPropertiesHook(ProjectOptions options,
            Dictionary<string, string> properties)
        {
            switch (options.SteeltoeVersion)
            {
                case SteeltoeVersion.Steeltoe31:
                    properties["SteeltoeVersion"] = "3.1.*";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.SteeltoeVersion),
                        options.SteeltoeVersion.ToString());
            }

            switch (options.Framework)
            {
                case Framework.Net60:
                    properties["TargetFramework"] = "net6.0";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Framework), options.Framework.ToString());
            }
        }

        protected override void AssertAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
            assertions.Add(AssertAppSettings);
        }

        private void AssertAppSettings(ProjectOptions options, AppSettings settings)
        {
            settings.Schema.Should().Be("https://steeltoe.io/schema/latest/schema.json");
        }

        protected override void AssertDevelopmentAppSettingsJsonHook(List<Action<ProjectOptions, AppSettings>> assertions)
        {
            assertions.Add(AssertDevelopmentAppSettings);
        }

        private void AssertDevelopmentAppSettings(ProjectOptions options, AppSettings settings)
        {
            settings.Schema.Should().Be("https://steeltoe.io/schema/latest/schema.json");
        }

        protected override void AssertLaunchSettingsHook(List<Action<ProjectOptions, LaunchSettings>> assertions)
        {
            assertions.Add(AssertLaunchSettings);
        }

        private void AssertLaunchSettings(ProjectOptions options, LaunchSettings settings)
        {
            if (options.Framework >= Framework.Net50)
            {
                settings.Profiles[Sandbox.Name].LaunchUrl.Should().Be("swagger");
            }
            else
            {
                settings.Profiles[Sandbox.Name].LaunchUrl.Should().Be("weatherforecast");
            }
        }
    }
}
