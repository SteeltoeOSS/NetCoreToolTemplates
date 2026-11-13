#if (DataProtectionRedisOption)
using Microsoft.AspNetCore.DataProtection;
#endif
#if (HasConnectorMongoDbInV4)
using Steeltoe.Connectors.MongoDb;
#endif
#if (HasConnectorCosmosDbInSteeltoeV4)
using Steeltoe.Connectors.CosmosDb;
#endif
#if (HasConnectorMySqlInSteeltoeV4)
using Steeltoe.Connectors.MySql;
#endif
#if (HasConnectorMySqlEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
#endif
#if (HasConnectorPostgreSqlInSteeltoeV4)
using Steeltoe.Connectors.PostgreSql;
#endif
#if (HasConnectorPostgreSqlEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.PostgreSql;
#endif
#if (HasConnectorRabbitMqInSteeltoeV4)
using Steeltoe.Connectors.RabbitMQ;
#endif
#if (HasAnyRedisInSteeltoeV4)
using Steeltoe.Connectors.Redis;
#endif
#if (HasConnectorSqlServerInSteeltoeV4)
using Steeltoe.Connectors.SqlServer;
#endif
#if (HasConnectorSqlServerEfCoreInSteeltoeV4)
using Steeltoe.Connectors.EntityFrameworkCore.SqlServer;
#endif
#if (HasDiscoveryConsulInSteeltoeV44)
using Steeltoe.Discovery.Consul;
#endif
#if (HasDiscoveryEurekaInSteeltoeV4)
using Steeltoe.Discovery.Eureka;
#endif
#if (HasHostingCloudFoundryInSteeltoeV4)
using Steeltoe.Configuration.CloudFoundry;
#endif
#if (HasConfigurationCloudConfigInSteeltoeV4)
using Steeltoe.Configuration.ConfigServer;
#endif
#if (HasConfigurationEncryptionInSteeltoeV4)
using Steeltoe.Configuration.Encryption;
#endif
#if (HasConfigurationPlaceholderInSteeltoeV4)
using Steeltoe.Configuration.Placeholder;
#endif
#if (HasConfigurationRandomValueInSteeltoeV4)
using Steeltoe.Configuration.RandomValue;
#endif
#if (HasConfigurationSpringBootInSteeltoeV4)
using Steeltoe.Configuration.SpringBoot;
#endif
#if (HasLoggingDynamicSerilogInSteeltoeV4)
using Steeltoe.Logging.DynamicSerilog;
#endif
#if (HasLoggingDynamicConsoleInSteeltoeV4)
using Steeltoe.Logging.DynamicConsole;
#endif
#if (HasManagementEndpointsInSteeltoeV4)
using Steeltoe.Management.Endpoint.Actuators.All;
#endif
#if (HasManagementTasksInSteeltoeV4)
using Steeltoe.Management.Tasks;
#endif
#if (DistributedTracingOption)
using Steeltoe.Management.Tracing;
#endif
#if (HasDataProtectionRedisInSteeltoeV4)
using Steeltoe.Security.DataProtection.Redis;
#endif
#if (RequiresProjectNamespaceImport)
using Company.WebApplication.CS;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#if (IsFrameworkNet100)
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
#endif
#if (HasAnySteeltoe)

// Configure Steeltoe components.
#endif
#if (HasConfigurationPlaceholderInSteeltoeV4)
builder.Configuration.AddPlaceholderResolver();
#endif
#if (HostingCloudFoundryOption)
builder.AddCloudFoundryConfiguration();
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
#if (HasConfigurationSpringBootInSteeltoeV4)
builder.Configuration.AddSpringBootFromEnvironmentVariable();
builder.Configuration.AddSpringBootFromCommandLine(args);
#endif
#if (HasLoggingDynamicSerilogInSteeltoeV4)
builder.Logging.AddDynamicSerilog();
#endif
#if (LoggingDynamicConsoleOption)
builder.Logging.AddDynamicConsole();
#endif
#if (HasDiscoveryConsulInSteeltoeV44)
builder.Services.AddConsulDiscoveryClient();
#endif
#if (HasDiscoveryEurekaInSteeltoeV4)
builder.Services.AddEurekaDiscoveryClient();
#endif
#if (HasConnectorMongoDbInV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MongoDb:Default:ConnectionString
builder.AddMongoDb();
#endif
#if (HasConnectorCosmosDbInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:CosmosDb:Default:ConnectionString
builder.AddCosmosDb();
#endif
#if (HasConnectorMySqlInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString
builder.AddMySql();
#endif
#if (HasConnectorMySqlEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));
#endif
#if (HasConnectorPostgreSqlInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString
builder.AddPostgreSql();
#endif
#if (HasConnectorPostgreSqlEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider));
#endif
#if (HasConnectorRabbitMqInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:RabbitMQ:Default:ConnectionString
builder.AddRabbitMQ();
#endif
#if (HasAnyRedisInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString
builder.AddRedis();
#endif
#if (DataProtectionRedisOption)
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("Company.WebApplication.CS");
#endif
#if (HasConnectorSqlServerInSteeltoeV4)
// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString
builder.AddSqlServer();
#endif
#if (HasConnectorSqlServerEfCoreInSteeltoeV4)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider));
#endif
#if (HasManagementEndpointsInSteeltoeV4)
builder.Services.AddAllActuators();
#endif
#if (HasManagementTasksInSteeltoeV4)
builder.Services.AddTask("run-me", (_, _) =>
{
    // Run this app with command-line argument: runtask=run-me
    Console.WriteLine("Hello from application task.");
    return Task.CompletedTask;
});
#endif
#if (HasDistributedTracingInSteeltoeV4)
builder.Services.AddTracingLogProcessor();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
#if (IsFrameworkNet100)
    app.MapOpenApi();
#endif
}

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
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

#if (HasManagementTasksInSteeltoeV4)
await app.RunWithTasksAsync(CancellationToken.None);
#else
app.Run();
#endif

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
