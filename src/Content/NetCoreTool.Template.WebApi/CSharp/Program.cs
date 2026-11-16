#if (HasDataProtectionRedis)
using Microsoft.AspNetCore.DataProtection;
#endif
#if (HasConnectorMongoDb)
using Steeltoe.Connectors.MongoDb;
#endif
#if (HasConnectorCosmosDb)
using Steeltoe.Connectors.CosmosDb;
#endif
#if (HasAnyConnectorMySql)
using Steeltoe.Connectors.MySql;
#endif
#if (HasConnectorMySqlEfCore)
using Steeltoe.Connectors.EntityFrameworkCore.MySql;
#endif
#if (HasAnyConnectorPostgreSql)
using Steeltoe.Connectors.PostgreSql;
#endif
#if (HasConnectorPostgreSqlEfCore)
using Steeltoe.Connectors.EntityFrameworkCore.PostgreSql;
#endif
#if (HasConnectorRabbitMq)
using Steeltoe.Connectors.RabbitMQ;
#endif
#if (HasAnyRedis)
using Steeltoe.Connectors.Redis;
#endif
#if (HasAnyConnectorSqlServer)
using Steeltoe.Connectors.SqlServer;
#endif
#if (HasConnectorSqlServerEfCore)
using Steeltoe.Connectors.EntityFrameworkCore.SqlServer;
#endif
#if (HasDiscoveryConsul)
using Steeltoe.Discovery.Consul;
#endif
#if (HasDiscoveryEureka)
using Steeltoe.Discovery.Eureka;
#endif
#if (HasConfigurationCloudFoundry)
using Steeltoe.Configuration.CloudFoundry;
#endif
#if (HasConfigurationConfigServer)
using Steeltoe.Configuration.ConfigServer;
#endif
#if (HasConfigurationEncryption)
using Steeltoe.Configuration.Encryption;
#endif
#if (HasConfigurationPlaceholder)
using Steeltoe.Configuration.Placeholder;
#endif
#if (HasConfigurationRandomValue)
using Steeltoe.Configuration.RandomValue;
#endif
#if (HasConfigurationSpringBoot)
using Steeltoe.Configuration.SpringBoot;
#endif
#if (HasLoggingDynamicConsole)
using Steeltoe.Logging.DynamicConsole;
#endif
#if (HasLoggingDynamicSerilog)
using Steeltoe.Logging.DynamicSerilog;
#endif
#if (HasManagementEndpoints)
using Steeltoe.Management.Endpoint.Actuators.All;
#endif
#if (HasManagementTasks)
using Steeltoe.Management.Tasks;
#endif
#if (HasDistributedTracing)
using Steeltoe.Management.Tracing;
#endif
#if (HasDataProtectionRedis)
using Steeltoe.Security.DataProtection.Redis;
#endif
#if (RequiresProjectNamespaceImport)
using Company.WebApplication.CS;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
#if (HasAnySteeltoe)

// Configure Steeltoe components.
#endif
#if (HasConfigurationPlaceholder)
builder.Configuration.AddPlaceholderResolver();
#endif
#if (HasConfigurationCloudFoundry)
builder.AddCloudFoundryConfiguration();
#endif
#if (HasConfigurationConfigServer)
builder.AddConfigServer();
#endif
#if (HasConfigurationEncryption)
builder.Configuration.AddDecryption();
#endif
#if (HasConfigurationRandomValue)
builder.Configuration.AddRandomValueSource();
#endif
#if (HasConfigurationSpringBoot)
builder.Configuration.AddSpringBootFromEnvironmentVariable();
builder.Configuration.AddSpringBootFromCommandLine(args);
#endif
#if (HasLoggingDynamicSerilog)
builder.Logging.AddDynamicSerilog();
#endif
#if (HasLoggingDynamicConsole)
builder.Logging.AddDynamicConsole();
#endif
#if (HasDiscoveryConsul)
builder.Services.AddConsulDiscoveryClient();
#endif
#if (HasDiscoveryEureka)
builder.Services.AddEurekaDiscoveryClient();
#endif
#if (HasConnectorMongoDb)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MongoDb:Default:ConnectionString
builder.AddMongoDb();
#endif
#if (HasConnectorCosmosDb)
// TODO: Add your connection string at configuration key: Steeltoe:Client:CosmosDb:Default:ConnectionString
builder.AddCosmosDb();
#endif
#if (HasAnyConnectorMySql)
// TODO: Add your connection string at configuration key: Steeltoe:Client:MySql:Default:ConnectionString
builder.AddMySql();
#endif
#if (HasConnectorMySqlEfCore)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseMySql(serviceProvider));
#endif
#if (HasAnyConnectorPostgreSql)
// TODO: Add your connection string at configuration key: Steeltoe:Client:PostgreSql:Default:ConnectionString
builder.AddPostgreSql();
#endif
#if (HasConnectorPostgreSqlEfCore)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseNpgsql(serviceProvider));
#endif
#if (HasConnectorRabbitMq)
// TODO: Add your connection string at configuration key: Steeltoe:Client:RabbitMQ:Default:ConnectionString
builder.AddRabbitMQ();
#endif
#if (HasAnyRedis)
// TODO: Add your connection string at configuration key: Steeltoe:Client:Redis:Default:ConnectionString
builder.AddRedis();
#endif
#if (HasDataProtectionRedis)
builder.Services.AddDataProtection().PersistKeysToRedis().SetApplicationName("Company.WebApplication.CS");
#endif
#if (HasAnyConnectorSqlServer)
// TODO: Add your connection string at configuration key: Steeltoe:Client:SqlServer:Default:ConnectionString
builder.AddSqlServer();
#endif
#if (HasConnectorSqlServerEfCore)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider));
#endif
#if (HasManagementEndpoints)
builder.Services.AddAllActuators();
#endif
#if (HasManagementTasks)
builder.Services.AddTask("run-me", (_, _) =>
{
    // Run this app with command-line argument: runtask=run-me
    Console.WriteLine("Hello from application task.");
    return Task.CompletedTask;
});
#endif
#if (HasDistributedTracing)
builder.Services.AddTracingLogProcessor();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

#if (HasManagementTasks)
await app.RunWithTasksAsync(CancellationToken.None);
#else
app.Run();
#endif

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
