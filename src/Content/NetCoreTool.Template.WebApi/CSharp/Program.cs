#if (HostingAzureSpringCloudOption)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (AnyHosting)
using Steeltoe.Common.Hosting;
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
#if (ConfigurationCloudConfigOption)
            .AddConfigServer()
#endif
#if (ConfigurationRandomValueOption)
            .ConfigureAppConfiguration(b => b.AddRandomValueSource())
#endif
#if (DynamicLogging)
            .ConfigureLogging((context, builder) => builder.AddDynamicConsole())
#endif


#if (AnyMessagingRabbitMq)
builder.Services.AddRabbitServices(true);
// Add Steeltoe RabbitAdmin services to get queues declared
builder.Service.AddRabbitAdmin();
// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Service.AddRabbitQueue(new Queue("steeltoe_message_queue"));
#endif
#if (MessagingRabbitMqClient)
// Add Steeltoe RabbitTemplate for sending/receiving
builder.Service.AddRabbitTemplate();
#endif
#if (MessagingRabbitMqListener)
// Add singleton that will process incoming messages
builder.Service.AddSingleton<RabbitListenerService>();
// Tell steeltoe about singleton so it can wire up queues with methods to process queues
builder.Service.AddRabbitListeners<RabbitListenerService>();
#endif
#if (ConnectorOAuthOption)
builder.Service.AddOAuthServiceOptions(Configuration);
#endif
#if (HostingCloudFoundryOption)
builder.Service.ConfigureCloudFoundryOptions(Configuration);
#endif
#if (DiscoveryEurekaOption)
builder.Service.AddDiscoveryClient(Configuration);
#endif
#if (ConnectorMongoDbOption)
builder.Service.AddMongoClient(Configuration);
#endif
#if (ConnectorMySqlOption)
builder.Service.AddMySqlConnection(Configuration);
#endif
#if (ConnectorMySqlEfCoreOption)
builder.Service.AddDbContext<SampleContext>(options => options.UseMySql(Configuration));
#endif
#if (ConnectorPostgreSqlOption)
builder.Service.AddPostgresConnection(Configuration);
#endif
#if (ConnectorPostgreSqlEfCoreOption)
builder.Service.AddDbContext<SampleContext>(options => options.UseNpgsql(Configuration));
#endif
#if (ConnectorRabbitMqOption)
builder.Service.AddRabbitMQConnection(Configuration);
#endif
#if (ConnectorRedisOption)
builder.Service.AddDistributedRedisCache(Configuration);
#endif
#if (ConnectorSqlServerOption)
builder.Service.AddSqlServerConnection(Configuration);
#endif
#if (CircuitBreakerHystrixOption)
builder.Service.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", Configuration);
builder.Service.AddHystrixMetricsStream(Configuration);
#endif
#if (ManagementEndpointsOption)
builder.Service.AddAllActuators(Configuration);
builder.Service.ActivateActuatorEndpoints();
#endif
#if (AnyTracing)
builder.Service.AddDistributedTracingAspNetCore();
#endif

builder.Service.AddControllers();
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

app.MapControllers();

app.Run();