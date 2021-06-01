# Developer Documentation

## Adding a New Project Option

A project option is an option provided to the `dotnet new` command that affects the generated project in some way.

In this example, we add a new project option named `hello-world` that logs a hello message when our application starts up.

Sample output of a generated project with the new `hello-world` project option:
```
$ dotnet new steeltoe-webapi --output MyHelloWorldApp --hello-world
$ dotnet run --project MyHelloWorldApp
info: MyHelloWorldApp.Startup[0]
      Hello World from MyHelloWorldApp
...
```

We'll use a [TDD](https://en.wikipedia.org/wiki/Test-driven_development) style to implement our new project option.
A major tenet of TDD is that you should first see your tests fail prior to making or implementing changes.
The nature of template development makes honoring this tenet a bit cumbersome, however we'll attempt to placate the TDD tenet somewhat by using 2 branches:
* one to write our tests and ultimately implement our project option
* one to develop our option to satisfy the tests

The general flow of the implementation is:
* create a basic test and define the option so that the test passes
* implement the template changes for the option and add tests to verify those changes

### Setting Up the Development Workspace

Clone this repository and create 2 branches:
```
$ git clone git@github.com:steeltoeoss-incubator/DotNetNewTemplates.git
$ cd DotNetNewTemplates
$ git branch hello-world              # ultimate branch for our new option
$ git branch hello-world-dev          # temporary development branch
```

### Creating a Test Class for the Project Option

Switch to the ultimate branch:
```
$ git switch hello-world
```

The `hello-world` project option is a boolean option, which in use will look something like:
```
$ dotnet new steeltoe-webapi                       # hello-world is false
$ dotnet new steeltoe-webapi --hello-world         # hello-world is true
$ dotnet new steeltoe-webapi --hello-world false   # hello-world is false
$ dotnet new steeltoe-webapi --hello-world true    # hello-world is true
```

There are several abstract test classes in the project to assist in option development.
The abstract class [ProjectOptionTest](https://github.com/steeltoeoss-incubator/DotNetNewTemplates/blob/main/test/DotNetNew.SteeltoeWebApi.Test/ProjectOptionTest.cs) should be used for project options such as `hello-world`.

The `ProjectOptionTest` constructor takes 3 arguments:
* option name
* option description
* logger (injected)

The description is what will be output by the `dotnet new` help engine.

Create the file `test/DotNetNew.SteeltoeWebApi.Test/HelloWorldOptionTest.cs`:
```c#
using Xunit.Abstractions;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test
{
    public class HelloWorldOptionTest : OptionTest
    {
        public HelloWorldOptionTest(ITestOutputHelper logger) : base("hello-world", "Say 'Hi' to the world", logger)
        {
        }
    }
}
```

Run the new test class, expecting several failures:
```
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest'
...
Failed!  - Failed:     9, Passed:     0, Skipped:     0, Total:     9, Duration: 5 s ...
```

We'll implement our project option and fix the broken tests in the `hello-world-dev` temporary branch.
Before proceeding to those steps, add the new test case to the ultimate branch:
```
$ git add test/DotNetNew.SteeltoeWebApi.Test/HelloWorldOptionTest.cs
$ git commit -m'Create hello-world test stub'
```

### Define the Project Option

Switch to the temporary branch.
```
$ git switch hello-world-dev
```

`dotnew new` options are defined as symbols in the file `.template.config/template.json`.
By this project's convention, project option symbol names are defined with the suffix `Option` so the new symbol will be named `HelloWorldOption`.

Add the new symbol to the the `symbols` object to `src/DotNetNew.WebApi/CSharp/.template.config/template.json`:
```json
  "symbols": {
...
    "HelloWorldOption": {
      "type": "parameter",
      "datatype": "bool",
      "description": "Say 'Hi' to the world",
      "defaultValue": "false"
    },
...
```

The symbol will be made available to our template C# code as a preprocessor directive which we'll leverage in later steps.

Now that symbol has been defined, we need to configure the `dotnew new` command so that the option `--hello-world` maps to the new symbol.
The option-to-symbol mapping is configured in the file `.template.config/dotnetcli.host.json`.

Add the new option to the the `symbolInfo` object to `src/DotNetNew.WebApi/CSharp/.template.config/dotnetcli.host.json`:
```json
  "symbolInfo": {
...
    "HelloWorldOption": {
      "longName": "hello-world",
      "shortName": ""
      },
...
```

If you haven't already, install the project template so that it can be invoked using `dotnet new`:
```
$ dotnet new --install src/DotNetNew.WebApi
```

Run the `steeltoe-webapi` template with the `--help` option to see the newly added option:
```
$ dotnet new steeltoe-webapi --help
...
Steeltoe Web API (C#)
...
  --hello-world           Say 'Hi' to the world
                          bool - Optional
                          Default: false
...
```

Add the changes to the temporary branch:
```
$ git add src/DotNetNew.WebApi/CSharp/.template.config/template.json
$ git add src/DotNetNew.WebApi/CSharp/.template.config/dotnetcli.host.json
$ git commit -m'Define the hello-world option'
```

### Testing the Project Option

Switch to the ultimate branch.
```
$ git switch hello-world
```

The `ProjectOptionTest` defines 3 categories of tests:
* _Smoke_
* _ProjectGeneration_
* _ProjectBuild_

We'll use the _Smoke_ test category to test the new project option.
The _Smoke_ test category includes a single test case that:
* runs `dotnet new steeltoe-webapi --help` and verifies an option description
* runs `dotnet new steeltoe-webapi --<option>` and verifies the command return code is 0

The _ProjectGeneration_ and _ProjectBuild_ test categories are covered later in this document.

Run the smoke test and see the test fail:
```
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest&Category=Smoke'
...
Failed!  - Failed:     1, Passed:     0, Skipped:     0, Total:     1, Duration: 1 s ...
```

Merge in the temporary branch and see the smoke test pass:
```
$ git merge hello-world-dev
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest&Category=Smoke'
Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 4 s ...
```

### Adding Project Option Behavior

Switch to the temporary branch.

```
$ git switch hello-world-dev
```

We'll edit `Startup.cs` so that a hello log message is output when the app starts.
The message will be logged from the `Configure` method so its signature will need to be changed to inject a logger.
There's also a corresponding `using` statement for the `ILogger` type.
However, we only want projects that are generated with the `--hello-world` option to have these changes so we'll use the C# `HelloWorldOption` preprocessor directive around our new code.
Note there are 2 `Configure` method signatures; one for `netcoreapp2.1` and one for everything else.

Edit `src/DotNetNew.WebApi/CSharp/Startup.cs`:
```C#
...
#if (HelloWorldOption)
using Microsoft.Extensions.Logging;
#endif
...
#if (FrameworkNetCoreApp21)
#if (HelloWorldOption)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
#else
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
#else
#if (HelloWorldOption)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
#else
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#endif
#endif
        {
#if (HelloWorldOption)
            logger.LogInformation("Hello, World, from {Name}", "Company.WebApplication1");
#endif
            if (env.IsDevelopment())
...
```

Create a couple projects to try out the `hello-world` option.

```
$ dotnet new steeltoe-webapi --output MyHelloWorldApp --hello-world
▶ dotnet run --project MyHelloWorldApp
info: MyHelloWorldApp.Startup[0]
      Hello, World, from MyHelloWorldApp
...

$ dotnet new steeltoe-webapi --output MyHelloWorldApp21 --hello-world --framework netcoreapp2.1
$ dotnet run --project MyHelloWorldApp21
info: MyHelloWorldApp21.Startup[0]
      Hello, World, from MyHelloWorldApp21
...
```

Add the changes to the temporary branch:
```
$ git src/DotNetNew.WebApi/CSharp/Startup.cs
$ git commit -m'Implement the hello-world option'
```

### Add Project Option Behavior Tests

Switch to the ultimate branch.
```
$ git switch hello-world
```

The `ProjectOptionTest` abstract class provides a _ProjectGeneration_ test category that tests an option with each valid combination of Steeltoe version and .NET framework.
The tests focus on generated project code, e.g. packages in the `.csproj` file and snippets in `Startup.cs`.
In all, 4 tests are run for an option:
* `dotnet new steeltoe-webapi --steeltoe 3.0.x --framework net5.0 --<option> --no-restore`
* `dotnet new steeltoe-webapi --steeltoe 3.0.x --framework netcoreapp3.1 --<option> --no-restore`
* `dotnet new steeltoe-webapi --steeltoe 2.5.x --framework netcoreapp3.1 --<option> --no-restore`
* `dotnet new steeltoe-webapi --steeltoe 2.5.x --framework netcoreapp2.1 --<option> --no-restore`

Subclasses of `ProjectOptionTest` can override various hooks to control what is validated during the tests.
The available hooks are:
* `AssertCsprojPackagesHook(SteeltoeVersion steeltoeVersion, Framework framework, List<(string, string)> packages)`
* `AssertCsprojPropertiesHook(SteeltoeVersion steeltoeVersion, Framework framework, Dictionary<string, string> properties)`
* `AssertProgramCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework, List<string> snippets)`
* `AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework, List<string> snippets)`
* `AssertValuesControllerCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework, List<string> snippets)`
* `AssertAppSettingsJsonHook(List<Action<SteeltoeVersion, Framework, AppSettings>> assertions)`
* `AssertLaunchSettingsHook(List<Action<SteeltoeVersion, Framework, LaunchSettings>> assertions)`

The `hello-world` _ProjectGeneration_ tests need to ensure that `Startup.cs` contains our new code when the option is specified.

Add the following override to the `test/DotNetNew.SteeltoeWebApi.Test/HelloWorldOptionTest.cs`:
```C#
        protected override void AssertStartupCsSnippetsHook(SteeltoeVersion steeltoeVersion, Framework framework, List<string> snippets)
        {
            snippets.Add("using Microsoft.Extensions.Logging;");
            switch (framework)
            {
                case Framework.NetCoreApp21:
                    snippets.Add("public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)");
                    break;
                default:
                    snippets.Add("public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)");
                    break;
            }

            snippets.Add($"logger.LogInformation(\"Hello, World, from {{Name}}\", \"{Sandbox.Name}\");");
        }
```

The order of the snippets in the list doesn't matter, but adding them in the order as they appear in the generated code makes it easier to follow.

Run the project generation tests and see the tests fail:
```
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest&Category=ProjectGeneration'
...
Failed!  - Failed:     4, Passed:     0, Skipped:     0, Total:     4, Duration: 3 s ...
```

Merge in the temporary branch and see the tests pass:
```
$ git merge hello-world-dev
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest&Category=ProjectGeneration'
Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 3 s ...
```

Add the project generation tests to the ultimate branch:
```
$ git add test/DotNetNew.SteeltoeWebApi.Test/HelloWorldOptionTest.cs
$ git commit -m'Add hello-world project generation tests'
```

### Run Project Build Tests

The _ProjectBuild_ test category tests ensure generated projects can be compiled.
Similarly to the _ProjectGeneration_ tests, an option is tested with valid combinations for Steeltoe versions and .NET frameworks.
For each combination, a test ensures the following command return codes are 0:
* `dotnet new steeltoe-webapi --steeltoe <version> --framework <framework> --<option>`
* `dotnet build /p:TreatWarningsAsErrors=True`


Run the project build tests and see if any test fail:
```
$ dotnet test --filter 'FullyQualifiedName~Steeltoe.DotNetNew.SteeltoeWebApi.Test.HelloWorldOptionTest&Category=ProjectGeneration'
...
Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 20 s ...
```

The project build tests pass so the implementation of the new project option is complete.

If a project build test should fail:
* fix the error in the development branch
* update the project generation tests if needed
* rerun the project build tests
