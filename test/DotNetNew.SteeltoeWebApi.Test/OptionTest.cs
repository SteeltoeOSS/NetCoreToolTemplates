using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Steeltoe.DotNetNew.Test.Utilities;
using Steeltoe.DotNetNew.Test.Utilities.Models;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public abstract class OptionTest
    {
        private readonly string _option;

        protected readonly ITestOutputHelper Logger;

        protected Sandbox Sandbox;

        protected OptionTest(string option, ITestOutputHelper logger)
        {
            _option = option;
            Logger = logger;
            new SteeltoeWebApiTemplateInstaller(Logger).Install();
            if (_option is not null)
            {
                Logger.WriteLine($"option: {_option}");
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void TestTemplate()
        {
            (await TemplateSandbox()).Dispose();
        }

        [Fact]
        [Trait("Category", "Functional")]
        public async void TestHelp()
        {
            using var sandbox = await TemplateSandbox("--help");
            AssertHelp(sandbox.CommandOutput);
        }

        protected virtual void AssertHelp(string help)
        {
            Logger.WriteLine($"testing help");
        }

        [Theory]
        [Trait("Category", "Functional")]
        [InlineData("3.0.2", "net5.0")]
        [InlineData("3.0.2", "netcoreapp3.1")]
        [InlineData("2.5.3", "netcoreapp3.1")]
        [InlineData("2.5.3", "netcoreapp2.1")]
        protected virtual async void TestProjectGeneration(string steeltoeOption, string frameworkOption)
        {
            Logger.WriteLine($"steeltoe/framework: {steeltoeOption}/{frameworkOption}");
            var steeltoe = ToSteeltoeEnum(steeltoeOption);
            var framework = ToFrameworkEnum(frameworkOption);
            Sandbox = await TemplateSandbox($"--steeltoe {steeltoeOption} --framework {frameworkOption}");
            try
            {
                await AssertProject(steeltoe, framework);
            }
            finally
            {
                Sandbox.Dispose();
                Sandbox = null;
            }
        }

        protected virtual async Task AssertProject(Steeltoe steeltoe, Framework framework)
        {
            Logger.WriteLine("asserting project");
            var project = await Sandbox.GetXmlDocumentAsync($"{Sandbox.Name}.csproj");
            var properties =
            (
                from e in project.Elements().Elements("PropertyGroup").Elements()
                select e
            ).ToArray().ToDictionary(e => e.Name.ToString(), e => e.Value);
            var packageRefs =
            (
                from e in project.Elements().Elements("ItemGroup").Elements("PackageReference").Attributes("Include")
                select e.Value
            ).ToArray();
            AssertCsproj(steeltoe, framework, properties, packageRefs);
            AssertProgramCs(steeltoe, framework, await Sandbox.GetFileTextAsync("Program.cs"));
            AssertStartupCs(steeltoe, framework, await Sandbox.GetFileTextAsync("Startup.cs"));
            AssertValuesControllerCs(steeltoe, framework,
                await Sandbox.GetFileTextAsync("Controllers/ValuesController.cs"));
            AssertAppSettingsJson(steeltoe, framework,
                await Sandbox.GetJsonDocumentAsync<AppSettings>("appsettings.json"));
            AssertLaunchSettingsJson(steeltoe, framework,
                await Sandbox.GetJsonDocumentAsync<LaunchSettings>("properties/launchSettings.json"));
        }

        protected virtual void AssertCsproj(Steeltoe steeltoe, Framework framework,
            Dictionary<string, string> properties, string[] packageRefs)
        {
            Logger.WriteLine("asserting .csproj");
        }

        protected virtual void AssertProgramCs(Steeltoe steeltoe, Framework framework, string source)
        {
            Logger.WriteLine("asserting Program.cs");
        }

        protected virtual void AssertStartupCs(Steeltoe steeltoe, Framework framework, string source)
        {
            Logger.WriteLine("asserting Startup.cs");
        }

        protected virtual void AssertValuesControllerCs(Steeltoe steeltoe, Framework framework, string source)
        {
            Logger.WriteLine("asserting Controllers/ValuesController.cs");
        }

        protected virtual void AssertAppSettingsJson(Steeltoe steeltoe, Framework framework, AppSettings settings)
        {
            Logger.WriteLine("asserting appsettings.json");
        }

        protected virtual void AssertLaunchSettingsJson(Steeltoe steeltoe, Framework framework, LaunchSettings settings)
        {
            Logger.WriteLine("asserting Properties/launchSettings.json");
        }

        protected async Task<Sandbox> TemplateSandbox(string args = "")
        {
            Assert.NotNull(args);
            var command = new StringBuilder("dotnet new stwebapi");
            if (!args.Contains("--help"))
            {
                if (!args.Contains("--no-restore"))
                {
                    command.Append(" --no-restore");
                }

                if (_option is not null)
                {
                    command.Append(" --").Append(_option);
                }
            }

            if (args.Length > 1)
            {
                command.Append(' ').Append(args);
            }

            var sandbox = new Sandbox(Logger);
            await sandbox.ExecuteCommandAsync(command.ToString());
            return sandbox;
        }

        protected bool IsSteeltoe2(string steeltoe)
        {
            return steeltoe.StartsWith("2.");
        }

        private static Steeltoe ToSteeltoeEnum(string steeltoe)
        {
            if (steeltoe.StartsWith("3."))
            {
                return Steeltoe.Steeltoe3;
            }

            if (steeltoe.StartsWith("2."))
            {
                return Steeltoe.Steeltoe2;
            }

            throw new ArgumentOutOfRangeException(nameof(steeltoe), steeltoe);
        }

        private static Framework ToFrameworkEnum(string framework)
        {
            return framework switch
            {
                "net5.0" => Framework.Net50,
                "netcoreapp3.1" => Framework.NetCoreApp31,
                "netcoreapp2.1" => Framework.NetCoreApp21,
                _ => throw new ArgumentOutOfRangeException(nameof(framework), framework)
            };
        }
    }
}
