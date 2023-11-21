using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
#if (CircuitBreakerHystrixOption)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (ConnectorMongoDbOption)
using Steeltoe.Connector.MongoDb;
#endif
#if (ConnectorMySqlOption)
using Steeltoe.Connector.MySql;
#endif
#if (ConnectorMySqlEfCoreOption)
using Steeltoe.Connector.MySql.EFCore;
#endif
#if (ConnectorOAuthOption)
using Steeltoe.Connector.OAuth;
#endif
#if (ConnectorPostgreSqlOption)
using Steeltoe.Connector.PostgreSql;
#endif
#if (ConnectorPostgreSqlEfCoreOption)
using Steeltoe.Connector.PostgreSql.EFCore;
#endif
#if (ConnectorRabbitMqOption)
using Steeltoe.Connector.RabbitMQ;
#endif
#if (ConnectorRedisOption)
using Steeltoe.Connector.Redis;
#endif
#if (ConnectorSqlServerOption)
using Steeltoe.Connector.SqlServer;
#endif
#if (DiscoveryEurekaOption)
using Steeltoe.Discovery.Client;
#endif
#if (HostingCloudFoundryOption)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (ManagementEndpointsOption)
using Steeltoe.Management.Endpoint;
#endif
#if (AnyTracing)
using Steeltoe.Management.Tracing;
#endif
#if (AnyMessagingRabbitMq)
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
#endif
#if (AnyEfCore)
using Company.WebApplication.CS.Models;
#endif
#if (MessagingRabbitMqListener)
using Company.WebApplication.CS.Services;
#endif

namespace Company.WebApplication.CS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if (AnyMessagingRabbitMq)
            // Add Steeltoe Rabbit services using JSON serialization
            // to use .NET default serialization, pass "false"
            services.AddRabbitServices(true);
            // Add Steeltoe RabbitAdmin services to get queues declared
            services.AddRabbitAdmin();
            // Add a queue to the message container that the rabbit admin will discover and declare at startup
            services.AddRabbitQueue(new Queue("steeltoe_message_queue"));
#endif
#if (MessagingRabbitMqClient)
            // Add Steeltoe RabbitTemplate for sending/receiving
            services.AddRabbitTemplate();
#endif
#if (MessagingRabbitMqListener)
            // Add singleton that will process incoming messages
            services.AddSingleton<RabbitListenerService>();
            // Tell steeltoe about singleton so it can wire up queues with methods to process queues
            services.AddRabbitListeners<RabbitListenerService>();
#endif
#if (ConnectorOAuthOption)
            services.AddOAuthServiceOptions(Configuration);
#endif
#if (HostingCloudFoundryOption)
            services.ConfigureCloudFoundryOptions(Configuration);
#endif
#if (DiscoveryEurekaOption)
            services.AddDiscoveryClient(Configuration);
#endif
#if (ConnectorMongoDbOption)
            services.AddMongoClient(Configuration);
#endif
#if (ConnectorMySqlOption)
            services.AddMySqlConnection(Configuration);
#endif
#if (ConnectorMySqlEfCoreOption)
            services.AddDbContext<SampleContext>(options => options.UseMySql(Configuration));
#endif
#if (ConnectorPostgreSqlOption)
            services.AddPostgresConnection(Configuration);
#endif
#if (ConnectorPostgreSqlEfCoreOption)
            services.AddDbContext<SampleContext>(options => options.UseNpgsql(Configuration));
#endif
#if (ConnectorRabbitMqOption)
            services.AddRabbitMQConnection(Configuration);
#endif
#if (ConnectorRedisOption)
            services.AddDistributedRedisCache(Configuration);
#endif
#if (ConnectorSqlServerOption)
            services.AddSqlServerConnection(Configuration);
#endif
#if (CircuitBreakerHystrixOption)
            services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", Configuration);
            services.AddHystrixMetricsStream(Configuration);
#endif
#if (ManagementEndpointsOption)
            services.AddAllActuators(Configuration);
            services.ActivateActuatorEndpoints();
#endif
#if (AnyTracing)
            services.AddDistributedTracingAspNetCore();
#endif
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company.WebApplication.CS", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company.WebApplication.CS"));
            }

#if (CircuitBreakerHystrixOption)
            app.UseHystrixRequestContext();
#endif
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
