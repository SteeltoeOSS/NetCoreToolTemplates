#if (DataProtectionRedisOption)
using Microsoft.AspNetCore.DataProtection;
#endif
#if (HasMessagingRabbitMqClientInSteeltoeV3)
using Microsoft.AspNetCore.Mvc;
#endif
#if (HasHostingAzureSpringCloudInSteeltoeV3)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (HasCircuitBreakerHystrixInSteeltoeV3)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (HasAnyNonAzureHostingInSteeltoeV3)
using Steeltoe.Common.Hosting;
#endif
#if (HasConnectorMongoDbInV3)
using Steeltoe.Connector.MongoDb;
#elif (HasConnectorMongoDbInV4)
using Steeltoe.Connectors.MongoDb;
#endif
#if (HasConnectorCosmosDbInSteeltoeV3)
using Steeltoe.Connector;
using Steeltoe.Connector.CosmosDb;
#elif (HasConnectorCosmosDbInSteeltoeV4)
using Steeltoe.Connectors.CosmosDb;
#endif
#if (HasConnectorMySqlInSteeltoeV3)
using Steeltoe.Connector.MySql;
#elif (HasConnectorMySqlInSteeltoeV4)
using Steeltoe.Connectors.MySql;
#endif
#if (HasConnectorMySqlEfCoreInSteeltoeV3)
using Steeltoe.Connector.MySql.EFCore;
#elif (HasConnectorMySqlEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
#endif
#if (HasConnectorOAuthInSteeltoeV3)
using Steeltoe.Connector.OAuth;
#endif
#if (HasConnectorPostgreSqlInSteeltoeV3)
using Steeltoe.Connector.PostgreSql;
#elif (HasConnectorPostgreSqlInSteeltoeV4)
using Steeltoe.Connectors.PostgreSql;
#endif
#if (HasConnectorPostgreSqlEfCoreInSteeltoeV3)
using Steeltoe.Connector.PostgreSql.EFCore;
#elif (HasConnectorPostgreSqlEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.PostgreSql;
#endif
#if (HasConnectorRabbitMqInSteeltoeV3)
using Steeltoe.Connector.RabbitMQ;
#elif (HasConnectorRabbitMqInSteeltoeV4)
using Steeltoe.Connectors.RabbitMQ;
#endif
#if (HasAnyRedisInSteeltoeV3)
using Steeltoe.Connector.Redis;
#elif (HasAnyRedisInSteeltoeV4)
using Steeltoe.Connectors.Redis;
#endif
#if (HasConnectorSqlServerInSteeltoeV3)
using Steeltoe.Connector.SqlServer;
#elif (HasConnectorSqlServerInSteeltoeV4)
using Steeltoe.Connectors.SqlServer;
#endif
#if (HasConnectorSqlServerEfCoreInSteeltoeV3)
using Steeltoe.Connector.SqlServer.EFCore;
#elif (HasConnectorSqlServerEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.SqlServer;
#endif
#if (HasAnyDiscoveryInSteeltoeV3)
using Steeltoe.Discovery.Client;
#else
#if (HasDiscoveryConsulInSteeltoeV44)
using Steeltoe.Discovery.Consul;
#endif
#if (HasDiscoveryEurekaInSteeltoeV4)
using Steeltoe.Discovery.Eureka;
#endif
#endif
#if (HasHostingCloudFoundryInSteeltoeV3)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#elif (HasHostingCloudFoundryInSteeltoeV4)
using Steeltoe.Configuration.CloudFoundry;
#endif
#if (HasConfigurationCloudConfigInSteeltoeV3)
using Steeltoe.Extensions.Configuration.ConfigServer;
#elif (HasConfigurationCloudConfigInSteeltoeV4)
using Steeltoe.Configuration.ConfigServer;
#endif
#if (HasConfigurationEncryptionInSteeltoeV4)
using Steeltoe.Configuration.Encryption;
#endif
#if (HasConfigurationPlaceholderInSteeltoeV3)
using Steeltoe.Extensions.Configuration.Placeholder;
#elif (HasConfigurationPlaceholderInSteeltoeV4)
using Steeltoe.Configuration.Placeholder;
#endif
#if (HasConfigurationRandomValueInSteeltoeV3)
using Steeltoe.Extensions.Configuration.RandomValue;
#elif (HasConfigurationRandomValueInSteeltoeV4)
using Steeltoe.Configuration.RandomValue;
#endif
#if (HasConfigurationSpringBootInSteeltoeV3)
using Steeltoe.Extensions.Configuration.SpringBoot;
#elif (HasConfigurationSpringBootInSteeltoeV4)
using Steeltoe.Configuration.SpringBoot;
#endif
#if (HasLoggingDynamicSerilogInSteeltoeV3)
using Steeltoe.Extensions.Logging.DynamicSerilog;
#elif (HasLoggingDynamicSerilogInSteeltoeV4)
using Steeltoe.Logging.DynamicSerilog;
#endif
#if (HasLoggingDynamicConsoleInSteeltoeV3)
using Steeltoe.Extensions.Logging;
#elif (HasLoggingDynamicConsoleInSteeltoeV4)
using Steeltoe.Logging.DynamicConsole;
#endif
#if (HasManagementEndpointsInSteeltoeV3)
using Steeltoe.Management.Endpoint;
#elif (HasManagementEndpointsInSteeltoeV4)
using Steeltoe.Management.Endpoint.Actuators.All;
#endif
#if (HasManagementTasksInSteeltoeV3)
using Steeltoe.Management.TaskCore;
#elif (HasManagementTasksInSteeltoeV4)
using Steeltoe.Management.Tasks;
#endif
#if (DistributedTracingOption)
using Steeltoe.Management.Tracing;
#endif
#if (HasAnyMessagingRabbitMqInSteeltoeV3)
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
#endif
#if (HasMessagingRabbitMqClientInSteeltoeV3)
using Steeltoe.Messaging.RabbitMQ.Core;
#endif
#if (HasDataProtectionRedisInSteeltoeV3)
using Steeltoe.Security.DataProtection;
#elif (HasDataProtectionRedisInSteeltoeV4)
using Steeltoe.Security.DataProtection.Redis;
#endif
#if (RequiresProjectNamespaceImport)
using Company.WebApplication.CS;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#if (IsFrameworkNet60 || IsFrameworkNet80)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#elif (IsFrameworkNet90 || IsFrameworkNet100)
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
#endif
#if (HasAnySteeltoe)

// Configure Steeltoe components.
#endif
#if (HasConfigurationPlaceholderInSteeltoeV3)
builder.AddPlaceholderResolver();
#elif (HasConfigurationPlaceholderInSteeltoeV4)
builder.Configuration.AddPlaceholderResolver();
#endif
#if (HasAnyNonAzureHostingInSteeltoeV3)
// Configures the application to listen on the port(s) provided by the cloud platform.
builder.UseCloudHosting();
#endif
#if (HostingCloudFoundryOption)
builder.AddCloudFoundryConfiguration();
#endif
#if (HasHostingAzureSpringCloudInSteeltoeV3)
builder.WebHost.UseAzureSpringCloudService();
#endif
#if (ConfigurationCloudConfigOption)
builder.AddConfigServer();
#endif
#if (HasConfigurationEncryptionInSteeltoeV4)
builder.Configuration.AddDecryption();
#endif
#if (ConfigurationRandomValueOption)
builder.Configuration.AddRandomValueSource();
#endif
#if (HasConfigurationSpringBootInSteeltoeV3)
builder.Configuration.AddSpringBootEnv();
builder.Configuration.AddSpringBootCmd(builder.Configuration);
#elif (HasConfigurationSpringBootInSteeltoeV4)
builder.Configuration.AddSpringBootFromEnvironmentVariable();
builder.Configuration.AddSpringBootFromCommandLine(args);
#endif
#if (HasLoggingDynamicSerilogInSteeltoeV3)
builder.AddDynamicSerilog();
#elif (HasLoggingDynamicSerilogInSteeltoeV4)
builder.Logging.AddDynamicSerilog();
#endif
#if (LoggingDynamicConsoleOption)
builder.Logging.AddDynamicConsole();
#endif
#if (HasAnyMessagingRabbitMqInSteeltoeV3)
builder.Services.AddRabbitServices(true);
// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();
// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue("steeltoe_message_queue"));
#endif
#if (HasMessagingRabbitMqClientInSteeltoeV3)
// Add Steeltoe RabbitTemplate for sending/receiving
builder.Services.AddRabbitTemplate();
#endif
#if (HasMessagingRabbitMqListenerInSteeltoeV3)
// Add singleton that will process incoming messages
builder.Services.AddSingleton<RabbitListenerService>();
// Tell steeltoe about singleton so it can wire up queues with methods to process queues
builder.Services.AddRabbitListeners<RabbitListenerService>();
#endif
#if (HasConnectorOAuthInSteeltoeV3)
builder.Services.AddOAuthServiceOptions(builder.Configuration);
#endif
#if (HasHostingCloudFoundryInSteeltoeV3)
builder.Services.ConfigureCloudFoundryOptions(builder.Configuration);
#endif
#if (HasAnyDiscoveryInSteeltoeV3)
// Registers this application with a service discovery provider (e.g., Eureka, Consul).
builder.Services.AddDiscoveryClient(builder.Configuration);
#else
#if (HasDiscoveryConsulInSteeltoeV44)
// Registers this application with Consul for service discovery.
// Port registration is automatically detected from the server's listening addresses.
builder.Services.AddConsulDiscoveryClient();
#endif
#if (HasDiscoveryEurekaInSteeltoeV4)
// Registers this application with Eureka for service discovery.
// Port registration is automatically detected from the server's listening addresses.
builder.Services.AddEurekaDiscoveryClient();
#endif
#endif
#if (HasConnectorMongoDbInV3)
// TODO: Add your connection string at configuration key: MongoDb:Client:ConnectionString
builder.Services.AddMongoClient(builder.Configuration);
#elif (HasConnectorMongoDbInV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MongoDb:Default:ConnectionString
builder.AddMongoDb();
#endif
#if (HasConnectorCosmosDbInSteeltoeV3)
// TODO: Add your connection string at configuration key: CosmosDb:Client:ConnectionString
var manager = new ConnectionStringManager(builder.Configuration);
var cosmosInfo = manager.Get<CosmosDbConnectionInfo>();
#elif (HasConnectorCosmosDbInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:CosmosDb:Default:ConnectionString
builder.AddCosmosDb();
#endif
#if (HasConnectorMySqlInSteeltoeV3)
// TODO: Add your connection string at configuration key: MySql:Client:ConnectionString
builder.Services.AddMySqlConnection(builder.Configuration);
#elif (HasConnectorMySqlInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString
builder.AddMySql();
#endif
#if (HasConnectorMySqlEfCoreInSteeltoeV3)
// TODO: Add your connection string at configuration key: MySql:Client:ConnectionString
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration));
#elif (HasConnectorMySqlEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));
#endif
#if (HasConnectorPostgreSqlInSteeltoeV3)
// TODO: Add your connection string at configuration key: Postgres:Client:ConnectionString
builder.Services.AddPostgresConnection(builder.Configuration);
#elif (HasConnectorPostgreSqlInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString
builder.AddPostgreSql();
#endif
#if (HasConnectorPostgreSqlEfCoreInSteeltoeV3)
// TODO: Add your connection string at configuration key: Postgres:Client:ConnectionString
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration));
#elif (HasConnectorPostgreSqlEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider));
#endif
#if (HasConnectorRabbitMqInSteeltoeV3)
// TODO: Add your connection string at configuration key: RabbitMq:Client:Url
builder.Services.AddRabbitMQConnection(builder.Configuration);
#elif (HasConnectorRabbitMqInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:RabbitMQ:Default:ConnectionString
builder.AddRabbitMQ();
#endif
#if (HasAnyRedisInSteeltoeV3)
// TODO: Add your connection string at configuration key: Redis:Client:ConnectionString
#endif
#if (HasConnectorRedisInSteeltoeV3)
builder.Services.AddDistributedRedisCache(builder.Configuration);
#endif
#if (HasDataProtectionRedisInSteeltoeV3)
builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);
#endif
#if (HasAnyRedisInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString
builder.AddRedis();
#endif
#if (DataProtectionRedisOption)
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("Company.WebApplication.CS");
#endif
#if (HasConnectorSqlServerInSteeltoeV3)
// TODO: Add your connection string at configuration key: SqlServer:Credentials:ConnectionString
builder.Services.AddSqlServerConnection(builder.Configuration);
#elif (HasConnectorSqlServerInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString
builder.AddSqlServer();
#endif
#if (HasConnectorSqlServerEfCoreInSteeltoeV3)
// TODO: Add your connection string at configuration key: SqlServer:Credentials:ConnectionString
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration));
#elif (HasConnectorSqlServerEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider));
#endif
#if (HasCircuitBreakerHystrixInSteeltoeV3)
builder.Services.AddHystrixCommand<HelloHystrixCommand>("ExampleCircuitBreakers", builder.Configuration);
builder.Services.AddHystrixMetricsStream(builder.Configuration);
#endif
#if (HasManagementEndpointsInSteeltoeV3)
builder.AddAllActuators();
#elif (HasManagementEndpointsInSteeltoeV4)
builder.Services.AddAllActuators();
#endif
#if (HasManagementTasksInSteeltoeV3)
builder.Services.AddTask("run-me", _ =>
{
    // Run this app with command-line argument: runtask=run-me
    Console.WriteLine("Hello from application task.");
});
#elif (HasManagementTasksInSteeltoeV4)
builder.Services.AddTask("run-me", (_, _) =>
{
    // Run this app with command-line argument: runtask=run-me
    Console.WriteLine("Hello from application task.");
    return Task.CompletedTask;
});
#endif
#if (HasDistributedTracingInSteeltoeV3)
builder.Services.AddDistributedTracingAspNetCore();
builder.Services.AddDistributedTracing();
#elif (HasDistributedTracingInSteeltoeV4)
builder.Services.AddTracingLogProcessor();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
#if (IsFrameworkNet60 || IsFrameworkNet80)
    app.UseSwagger();
    app.UseSwaggerUI();
#elif (IsFrameworkNet90 || IsFrameworkNet100)
    app.MapOpenApi();
#endif
}

#if (HasCircuitBreakerHystrixInSteeltoeV3)
app.UseHystrixRequestContext();
#endif
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
#if (IsFrameworkNet60)
            DateTime.Now.AddDays(index),
#else
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
#endif
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
#if (IsFrameworkNet80)
.WithName("GetWeatherForecast")
.WithOpenApi();
#else
.WithName("GetWeatherForecast");
#endif

#if (HasMessagingRabbitMqClientInSteeltoeV3)
app.MapGet("/sendtoqueue", ([FromServices] RabbitTemplate rabbitTemplate, [FromServices] RabbitAdmin rabbitAdmin) =>
{
    var message = "Hi there from over here.";
    rabbitTemplate.ConvertAndSend("steeltoe_message_queue", message);
    app.Logger.LogInformation("Sending message '{Message}' to queue 'steeltoe_message_queue'", message);
    return "Message sent to queue.";
})
.WithName("SendToQueue");

#endif
#if (HasManagementTasksInSteeltoeV3)
app.RunWithTasks();
#elif (HasManagementTasksInSteeltoeV4)
await app.RunWithTasksAsync(CancellationToken.None);
#else
app.Run();
#endif

#if (IsFrameworkNet60)
internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
#else
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
#endif
