using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (!FrameworkNetCoreApp21)
using Microsoft.Extensions.Hosting;
#endif
#if (FrameworkNet50)
using Microsoft.OpenApi.Models;
#endif
#if (HystrixOption)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (MongoDbOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MongoDb;
#endif
#if (MongoDbOption && !Steeltoe2)
using Steeltoe.Connector.MongoDb;
#endif
#if (MySqlOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MySql;
#endif
#if (MySqlOption && !Steeltoe2)
using Steeltoe.Connector.MySql;
#endif
#if (MySqlEfCoreOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
#endif
#if (MySqlEfCoreOption && !Steeltoe2)
using Steeltoe.Connector.MySql.EFCore;
#endif
#if (OAuthOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.OAuth;
#endif
#if (OAuthOption && !Steeltoe2)
using Steeltoe.Connector.OAuth;
#endif
#if (PostgreSqlOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.PostgreSql;
#endif
#if (PostgreSqlOption && !Steeltoe2)
using Steeltoe.Connector.PostgreSql;
#endif
#if (PostgreSqlEfCoreOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;
#endif
#if (PostgreSqlEfCoreOption && !Steeltoe2)
using Steeltoe.Connector.PostgreSql.EFCore;
#endif
#if (RabbitMqOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.RabbitMQ;
#endif
#if (RabbitMqOption && !Steeltoe2)
using Steeltoe.Connector.RabbitMQ;
#endif
#if (RedisOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.Redis;
#endif
#if (RedisOption && !Steeltoe2)
using Steeltoe.Connector.Redis;
#endif
#if (SqlServerOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.SqlServer;
#endif
#if (SqlServerOption && !Steeltoe2)
using Steeltoe.Connector.SqlServer;
#endif
#if (EurekaOption)
using Steeltoe.Discovery.Client;
#endif
#if (CloudFoundryHostingOption)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (Steeltoe2ManagementEndpoints)
using Steeltoe.Management.CloudFoundry;
#endif
#if (Steeltoe3ManagementEndpoints)
using Steeltoe.Management.Endpoint;
#endif
#if (AnyEfCore)
using Company.WebApplication1.Models;
#endif

#if (MessagingRabbitMqOption)
using Company.WebApplication1.Services;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
#endif

namespace Company.WebApplication1
{
    public class Startup
    {
#if (MessagingRabbitMqOption)
#endif
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if (OAuthOption)
            services.AddOAuthServiceOptions(Configuration);
#endif
#if (CloudFoundryHostingOption)
            services.ConfigureCloudFoundryOptions(Configuration);
#endif
#if (EurekaOption)
            services.AddDiscoveryClient(Configuration);
#endif
#if (MongoDbOption)
            services.AddMongoClient(Configuration);
#endif
#if (MySqlOption)
            services.AddMySqlConnection(Configuration);
#endif
#if (MySqlEfCoreOption)
            services.AddDbContext<SampleContext>(options => options.UseMySql(Configuration));
#endif
#if (PostgreSqlOption)
            services.AddPostgresConnection(Configuration);
#endif
#if (PostgreSqlEfCoreOption)
            services.AddDbContext<SampleContext>(options => options.UseNpgsql(Configuration));
#endif
#if (RabbitMqOption)
            services.AddRabbitMQConnection(Configuration);
#endif
#if (RedisOption)
            services.AddDistributedRedisCache(Configuration);
#endif
#if (SqlServerOption)
            services.AddSqlServerConnection(Configuration);
#endif
#if (HystrixOption)
            services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", Configuration);
            services.AddHystrixMetricsStream(Configuration);
#endif
#if (MessagingRabbitMqOption)
            // Add some queues to the container that the rabbit admin will discover and declare at startup
            services.AddRabbitQueue(new Queue(Queues.ReceiveAndConvertQueue));
            services.AddRabbitQueue(new Queue(Queues.InferredMessageQueue));
            services.AddRabbitQueue(new Queue(Queues.InferredSpecialMessageQueue));

            // Add singleton that will process incoming messages
            services.AddSingleton<RabbitMessagesListener>();

            // Tell steeltoe about singleton so it can wire up queues with methods to process queues (i.e., RabbitListenerAttribute)
            services.AddRabbitListeners<RabbitMessagesListener>();
#endif
#if (Steeltoe2ManagementEndpoints)
            services.AddCloudFoundryActuators(Configuration);
#endif
#if (Steeltoe3ManagementEndpoints)
            services.AddAllActuators(Configuration);
#endif
#if (FrameworkNetCoreApp21)
            services.AddMvc();
#else
            services.AddControllers();
#endif
#if (FrameworkNet50)
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company.WebApplication1", Version = "v1" });
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#if (FrameworkNetCoreApp21)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#else
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#endif
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if (FrameworkNet50)
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company.WebApplication1 v1"));
#endif
            }

#if (EurekaOption)
            app.UseDiscoveryClient();
#endif
#if (HystrixOption)
            app.UseHystrixRequestContext();
            app.UseHystrixMetricsStream();
#endif
#if (Steeltoe2ManagementEndpoints)
            app.UseCloudFoundryActuators();
#endif
#if (FrameworkNetCoreApp21)
            app.UseMvc();
#else
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
#endif
        }
    }
}