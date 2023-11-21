namespace Company.WebApplication.FS

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models
#if (CircuitBreakerHystrixOption)
open Steeltoe.CircuitBreaker.Hystrix
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
#if (Steeltoe3ManagementEndpoints)
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

#if (NeedsSelf)
type Startup(configuration: IConfiguration) as self =
#else
type Startup(configuration: IConfiguration) =
#endif
    member _.Configuration = configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member _.ConfigureServices(services: IServiceCollection) =
#if (AnyMessagingRabbitMq)
        // Add Steeltoe Rabbit services using JSON serialization
        // to use .NET default serialization, pass "false"
        services.AddRabbitServices(true) |> ignore
        // Add Steeltoe RabbitAdmin services to get queues declared
        services.AddRabbitAdmin() |> ignore
        // Add a queue to the message container that the rabbit admin will discover and declare at startup
        services.AddRabbitQueue(new Queue("steeltoe_message_queue")) |> ignore
#endif
#if (MessagingRabbitMqClient)
        // Add Steeltoe RabbitTemplate for sending/receiving
        services.AddRabbitTemplate() |> ignore
#endif
#if (MessagingRabbitMqListener)
        // Add singleton that will process incoming messages
        services.AddSingleton<RabbitListenerService>() |> ignore
        // Tell steeltoe about singleton so it can wire up queues with methods to process queues
        services.AddRabbitListeners<RabbitListenerService>() |> ignore
#endif
#if (ConnectorOAuthOption)
        services.AddOAuthServiceOptions(self.Configuration) |> ignore
#endif
#if (HostingCloudFoundryOption)
        services.ConfigureCloudFoundryOptions(self.Configuration) |> ignore
#endif
#if (DiscoveryEurekaOption)
        services.AddDiscoveryClient(self.Configuration) |> ignore
#endif
#if (ConnectorMongoDbOption)
        services.AddMongoClient(self.Configuration) |> ignore
#endif
#if (ConnectorMySqlOption)
        services.AddMySqlConnection(self.Configuration) |> ignore
#endif
#if (ConnectorMySqlEfCoreOption)
        services.AddDbContext<SampleContext>(fun options -> options.UseMySql(self.Configuration) |> ignore) |> ignore
#endif
#if (ConnectorPostgreSqlOption)
        services.AddPostgresConnection(self.Configuration) |> ignore
#endif
#if (ConnectorPostgreSqlEfCoreOption)
        services.AddDbContext<SampleContext>(fun options -> options.UseNpgsql(self.Configuration) |> ignore) |> ignore
#endif
#if (ConnectorRabbitMqOption)
        services.AddRabbitMQConnection(self.Configuration) |> ignore
#endif
#if (ConnectorRedisOption)
        services.AddDistributedRedisCache(self.Configuration) |> ignore
#endif
#if (ConnectorSqlServerOption)
        services.AddSqlServerConnection(self.Configuration) |> ignore
#endif
#if (CircuitBreakerHystrixOption)
        services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", self.Configuration)
        services.AddHystrixMetricsStream(self.Configuration)
#endif
#if (Steeltoe3ManagementEndpoints)
        services.AddAllActuators(self.Configuration) |> ignore
#if (!Steeltoe30)
        services.ActivateActuatorEndpoints()
#endif
#endif
#if (AnyTracing)
#if (Steeltoe30)
        services.AddDistributedTracing() |> ignore
#else
        services.AddDistributedTracingAspNetCore() |> ignore
#endif
#endif
        // Add framework services.
        services.AddControllers() |> ignore
        let info = OpenApiInfo()
        info.Title <- "Company.WebApplication.FS"
        info.Version <- "v1"
        services.AddSwaggerGen(fun c -> c.SwaggerDoc("v1", info)) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore
            app.UseSwaggerUI(fun c -> c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company.WebApplication.FS")) |> ignore
#if (DiscoveryEurekaOption)
#if (Steeltoe30)
        app.UseDiscoveryClient() |> ignore
#endif
#endif
#if (CircuitBreakerHystrixOption)
        app.UseHystrixRequestContext() |> ignore
#if (Steeltoe30)
        app.UseHystrixMetricsStream() |> ignore
#endif
#endif
        app.UseHttpsRedirection()
           .UseRouting()
           .UseAuthorization()
           .UseEndpoints(fun endpoints ->
                endpoints.MapControllers() |> ignore
#if (Steeltoe3ManagementEndpoints)
#if (Steeltoe30)
                endpoints.MapAllActuators() |> ignore
#endif
#endif
            ) |> ignore
