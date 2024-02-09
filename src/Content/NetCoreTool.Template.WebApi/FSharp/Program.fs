namespace Company.WebApplication.FS

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models
#if (HostingAzureSpringCloudOption)
open Microsoft.Azure.SpringCloud.Client
#endif
#if (CircuitBreakerHystrixOption)
open Steeltoe.CircuitBreaker.Hystrix
#endif
#if (AnyHosting)
open Steeltoe.Common.Hosting
#endif
#if (ConnectorMongoDbOption)
open Steeltoe.Connector.MongoDb
#endif
#if (ConnectorMySqlEfCoreOption)
open Steeltoe.Connector.MySql.EFCore
#endif
#if (ConnectorMySqlOption)
open Steeltoe.Connector.MySql
#endif
#if (ConnectorOAuthOption)
open Steeltoe.Connector.OAuth
#endif
#if (ConnectorPostgreSqlOption)
open Steeltoe.Connector.PostgreSql
#endif
#if (ConnectorPostgreSqlEfCoreOption)
open Steeltoe.Connector.PostgreSql.EFCore
#endif
#if (ConnectorRabbitMqOption)
open Steeltoe.Connector.RabbitMQ
#endif
#if (ConnectorRedisOption)
open Steeltoe.Connector.Redis
#endif
#if (ConnectorSqlServerOption)
open Steeltoe.Connector.SqlServer
#endif
#if (DiscoveryEurekaOption)
open Steeltoe.Discovery.Client
#endif
#if (HostingCloudFoundryOption)
open Steeltoe.Extensions.Configuration.CloudFoundry
#endif
#if (ConfigurationCloudConfigOption)
open Steeltoe.Extensions.Configuration.ConfigServer
#endif
#if (ConfigurationPlaceholderOption)
open Steeltoe.Extensions.Configuration.Placeholder
#endif
#if (ConfigurationRandomValueOption)
open Steeltoe.Extensions.Configuration.RandomValue
#endif
#if (DynamicLogging)
open Steeltoe.Extensions.Logging
#endif
#if (ManagementEndpointsOption)
open Steeltoe.Management.Endpoint
#endif
#if (AnyTracing)
open Steeltoe.Management.Tracing
#endif
#if (AnyMessagingRabbitMq)
open Steeltoe.Messaging.RabbitMQ.Config
open Steeltoe.Messaging.RabbitMQ.Extensions
#endif
#if (AnyEfCore)
open Company.WebApplication.FS.Models
#endif
#if (MessagingRabbitMqListener)
open Company.WebApplication.FS.Services
#endif

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)
#if (ConfigurationPlaceholderOption)
                         .AddPlaceholderResolver()
#endif
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
        builder.Configuration.AddConfigServer() |> ignore
#endif
#if (ConfigurationRandomValueOption)
        builder.Configuration.AddRandomValueSource() |> ignore
#endif
#if (DynamicLogging)
        builder.Logging.AddDynamicConsole() |> ignore
#endif

#if (HostingAzureSpringCloudOption)
        builder.WebHost.UseAzureSpringCloudService() |> ignore
#endif

// This method gets called by the runtime. Use this method to add services to the container.
#if (AnyMessagingRabbitMq)
        // Add Steeltoe Rabbit services using JSON serialization
        // to use .NET default serialization, pass "false"
        builder.Services.AddRabbitServices(true) |> ignore
        // Add Steeltoe RabbitAdmin services to get queues declared
        builder.Services.AddRabbitAdmin() |> ignore
        // Add a queue to the message container that the rabbit admin will discover and declare at startup
        builder.Services.AddRabbitQueue(new Queue("steeltoe_message_queue")) |> ignore
#endif
#if (MessagingRabbitMqClient)
        // Add Steeltoe RabbitTemplate for sending/receiving
        builder.Services.AddRabbitTemplate() |> ignore
#endif
#if (MessagingRabbitMqListener)
        // Add singleton that will process incoming messages
        builder.Services.AddSingleton<RabbitListenerService>() |> ignore
        // Tell steeltoe about singleton so it can wire up queues with methods to process queues
        builder.Services.AddRabbitListeners<RabbitListenerService>() |> ignore
#endif
#if (ConnectorOAuthOption)
        builder.Services.AddOAuthServiceOptions(builder.Configuration) |> ignore
#endif
#if (HostingCloudFoundryOption)
        builder.Services.ConfigureCloudFoundryOptions(builder.Configuration) |> ignore
#endif
#if (DiscoveryEurekaOption)
        builder.Services.AddDiscoveryClient(builder.Configuration) |> ignore
#endif
#if (ConnectorMongoDbOption)
        builder.Services.AddMongoClient(builder.Configuration) |> ignore
#endif
#if (ConnectorMySqlOption)
        builder.Services.AddMySqlConnection(builder.Configuration) |> ignore
#endif
#if (ConnectorMySqlEfCoreOption)
        builder.Services.AddDbContext<SampleContext>(fun options -> options.UseMySql(builder.Configuration) |> ignore) |> ignore
#endif
#if (ConnectorPostgreSqlOption)
        builder.Services.AddPostgresConnection(builder.Configuration) |> ignore
#endif
#if (ConnectorPostgreSqlEfCoreOption)
        builder.Services.AddDbContext<SampleContext>(fun options -> options.UseNpgsql(builder.Configuration) |> ignore) |> ignore
#endif
#if (ConnectorRabbitMqOption)
        builder.Services.AddRabbitMQConnection(builder.Configuration) |> ignore
#endif
#if (ConnectorRedisOption)
        builder.Services.AddDistributedRedisCache(builder.Configuration) |> ignore
#endif
#if (ConnectorSqlServerOption)
        builder.Services.AddSqlServerConnection(builder.Configuration) |> ignore
#endif
#if (CircuitBreakerHystrixOption)
        builder.Services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", builder.Configuration)
        builder.Services.AddHystrixMetricsStream(builder.Configuration)
#endif
#if (ManagementEndpointsOption)
        builder.Services.AddAllActuators(builder.Configuration) |> ignore
        builder.Services.ActivateActuatorEndpoints()
#endif
#if (AnyTracing)
        builder.Services.AddDistributedTracingAspNetCore() |> ignore
#endif

        // Add framework services.
        builder.Services.AddControllers() |> ignore
        builder.Services.AddEndpointsApiExplorer() |> ignore
        builder.Services.AddSwaggerGen() |> ignore

        let app = builder.Build()
        
        if (builder.Environment.IsDevelopment()) then
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore

        #if (CircuitBreakerHystrixOption)
        app.UseHystrixRequestContext() |> ignore
        #endif
        app.UseHttpsRedirection() |> ignore
        app.UseRouting() |> ignore

        app.MapControllers() |> ignore
        app.Run()
        exitCode
