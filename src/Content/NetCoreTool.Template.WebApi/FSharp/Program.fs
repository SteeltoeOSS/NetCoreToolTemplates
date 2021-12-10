namespace Company.WebApplication.FS

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
#if (HostingAzureSpringCloudOption)
open Microsoft.Azure.SpringCloud.Client
#endif
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
#if (AnyHosting)
open Steeltoe.Common.Hosting
#endif
#if (HostingCloudFoundryOption)
open Steeltoe.Extensions.Configuration.CloudFoundry
#endif
#if (ConfigurationCloudConfigOption)
open Steeltoe.Extensions.Configuration.ConfigServer
#endif
#if (ConfigurationPlaceholderOption)
#if (Steeltoe2)
open Steeltoe.Extensions.Configuration.PlaceholderCore;
#else
open Steeltoe.Extensions.Configuration.Placeholder;
#endif
#endif
#if (ConfigurationRandomValueOption)
open Steeltoe.Extensions.Configuration.RandomValue
#endif
#if (DynamicLogging)
open Steeltoe.Extensions.Logging
#endif

module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host.CreateDefaultBuilder(args)
#if (ConfigurationPlaceholderOption)
            .AddPlaceholderResolver()
#endif
#if (HostingAzureSpringCloudOption)
            .UseAzureSpringCloudService()
#endif
#if (ConfigurationRandomValueOption)
            .ConfigureAppConfiguration(fun builder -> builder.AddRandomValueSource() |> ignore)
#endif
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder.UseStartup<Startup>() |> ignore
            )

    [<EntryPoint>]
    let main args =
        CreateHostBuilder(args)
#if (AnyHosting)
#if (HostingCloudOption)
            .UseCloudHosting(8080)
#else
            .UseCloudHosting()
#endif
#endif
#if (HostingCloudFoundryOption)
            .AddCloudFoundryConfiguration()
#endif
#if (ConfigurationCloudConfigOption)
            .AddConfigServer()
#endif
#if (DynamicLogging)
            .ConfigureLogging(fun (builder) -> builder.AddDynamicConsole() |> ignore)
#endif
            .Build().Run()

        exitCode
