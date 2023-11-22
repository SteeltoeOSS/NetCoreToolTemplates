#if (HostingAzureSpringCloudOption)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (CircuitBreakerHystrixOption)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (AnyHosting)
using Steeltoe.Common.Hosting;
#endif
#if(ConnectorMongoDbOption)
using Steeltoe.Connector.MongoDb;
#endif
#if(ConnectorMySqlOption)
using Steeltoe.Connector.MySql;
#endif
#if(ConnectorMySqlEfCoreOption)
using Steeltoe.Connector.MySql.EFCore;
#endif
#if(ConnectorOAuthOption)
using Steeltoe.Connector.OAuth;
#endif
#if(ConnectorPostgreSqlOption)
using Steeltoe.Connector.PostgreSql;
#endif
#if(ConnectorPostgreSqlEfCoreOption)
using Steeltoe.Connector.PostgreSql.EFCore;
#endif
#if(ConnectorRabbitMqOption)
using Steeltoe.Connector.RabbitMQ;
#endif
#if(ConnectorRedisOption)
using Steeltoe.Connector.Redis;
#endif
#if(ConnectorSqlServerOption)
using Steeltoe.Connector.SqlServer;
#endif
#if (DiscoveryEurekaOption)
using Steeltoe.Discovery.Client;
#endif
#if (HostingCloudFoundryOption)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (ConfigurationCloudConfigOption)
using Steeltoe.Extensions.Configuration.ConfigServer;
#endif
#if (ConfigurationPlaceholderOption)
using Steeltoe.Extensions.Configuration.Placeholder;
#endif
#if (ConfigurationRandomValueOption)
using Steeltoe.Extensions.Configuration.RandomValue;
#endif
#if (DynamicLogging)
using Steeltoe.Extensions.Logging;
#endif
#if(ManagementEndpointsOption)
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Tracing;
#endif

var builder = WebApplication.CreateBuilder(args)
#if (ConfigurationPlaceholderOption)
    .AddPlaceholderResolver()
#endif
#if (HostingAzureSpringCloudOption)
    .UseAzureSpringCloudService()
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
;

#if (ConfigurationCloudConfigOption)
builder.Configuration.AddConfigServer();
#endif
#if (ConfigurationRandomValueOption)
builder.Configuration.AddRandomValueSource();
#endif
#if (DynamicLogging)
builder.Logging.AddDynamicConsole();
#endif

#if (AnyMessagingRabbitMq)
builder.Services.AddRabbitServices(true);
// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();
// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue("steeltoe_message_queue"));
#endif
#if (MessagingRabbitMqClient)
// Add Steeltoe RabbitTemplate for sending/receiving
builder.Services.AddRabbitTemplate();
#endif
#if (MessagingRabbitMqListener)
// Add singleton that will process incoming messages
builder.Services.AddSingleton<RabbitListenerService>();
// Tell steeltoe about singleton so it can wire up queues with methods to process queues
builder.Services.AddRabbitListeners<RabbitListenerService>();
#endif
#if (ConnectorOAuthOption)
builder.Services.AddOAuthServiceOptions(builder.Configuration);
#endif
#if (HostingCloudFoundryOption)
builder.Services.ConfigureCloudFoundryOptions(builder.Configuration);
#endif
#if (DiscoveryEurekaOption)
builder.Services.AddDiscoveryClient(builder.Configuration);
#endif
#if (ConnectorMongoDbOption)
builder.Services.AddMongoClient(builder.Configuration);
#endif
#if (ConnectorMySqlOption)
builder.Services.AddMySqlConnection(builder.Configuration);
#endif
#if (ConnectorMySqlEfCoreOption)
builder.Services.AddDbContext<SampleContext>(options => options.UseMySql(builder.Configuration));
#endif
#if (ConnectorPostgreSqlOption)
builder.Services.AddPostgresConnection(builder.Configuration);
#endif
#if (ConnectorPostgreSqlEfCoreOption)
builder.Services.AddDbContext<SampleContext>(options => options.UseNpgsql(builder.Configuration));
#endif
#if (ConnectorRabbitMqOption)
builder.Services.AddRabbitMQConnection(builder.Configuration);
#endif
#if (ConnectorRedisOption)
builder.Services.AddDistributedRedisCache(builder.Configuration);
#endif
#if (ConnectorSqlServerOption)
builder.Services.AddSqlServerConnection(builder.Configuration);
#endif
#if (CircuitBreakerHystrixOption)
builder.Services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", builder.Configuration);
builder.Services.AddHystrixMetricsStream(builder.Configuration);
#endif
#if (ManagementEndpointsOption)
builder.Services.AddAllActuators(builder.Configuration);
builder.Services.ActivateActuatorEndpoints();
#endif
#if (AnyTracing)
builder.Services.AddDistributedTracingAspNetCore();
#endif
#if (FrameworkNet60)
builder.Services.AddControllers();
#endif
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#if (CircuitBreakerHystrixOption)
app.UseHystrixRequestContext();
#endif
app.UseHttpsRedirection();
app.UseRouting();

#if (FrameworkNet60)
app.MapControllers();
#elseif (FrameworkNet80)
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
.WithName("GetWeatherForecast").WithOpenApi();

#if (MessagingRabbitMqClient)
app.MapGet("/sendtoqueue", ([FromServices] RabbitTemplate rabbitTemplate, [FromServices] RabbitAdmin rabbitAdmin) =>
{
    var msg = "Hi there from over here.";
    rabbitTemplate.ConvertAndSend("steeltoe_message_queue", msg);
    app.Logger.LogInformation($"Sending message '{msg}' to queue 'steeltoe_message_queue'");
    return "Message sent to queue.";
})
.WithName("SendToQueue");
#endif
#endif

app.Run();

#if (FrameworkNet80)
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
#endif
