using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (FrameworkNet50)
using Microsoft.OpenApi.Models;
#endif
#if (CircuitBreakerHystrixOption)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (ConnectorMongoDbOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MongoDb;
#endif
#if (ConnectorMongoDbOption && !Steeltoe2)
using Steeltoe.Connector.MongoDb;
#endif
#if (ConnectorMySqlOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MySql;
#endif
#if (ConnectorMySqlOption && !Steeltoe2)
using Steeltoe.Connector.MySql;
#endif
#if (ConnectorMySqlEfCoreOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
#endif
#if (ConnectorMySqlEfCoreOption && !Steeltoe2)
using Steeltoe.Connector.MySql.EFCore;
#endif
#if (ConnectorOAuthOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.OAuth;
#endif
#if (ConnectorOAuthOption && !Steeltoe2)
using Steeltoe.Connector.OAuth;
#endif
#if (ConnectorPostgreSqlOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.PostgreSql;
#endif
#if (ConnectorPostgreSqlOption && !Steeltoe2)
using Steeltoe.Connector.PostgreSql;
#endif
#if (ConnectorPostgreSqlEfCoreOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;
#endif
#if (ConnectorPostgreSqlEfCoreOption && !Steeltoe2)
using Steeltoe.Connector.PostgreSql.EFCore;
#endif
#if (ConnectorRabbitMqOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.RabbitMQ;
#endif
#if (ConnectorRabbitMqOption && !Steeltoe2)
using Steeltoe.Connector.RabbitMQ;
#endif
#if (ConnectorRedisOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.Redis;
#endif
#if (ConnectorRedisOption && !Steeltoe2)
using Steeltoe.Connector.Redis;
#endif
#if (ConnectorSqlServerOption && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.SqlServer;
#endif
#if (ConnectorSqlServerOption && !Steeltoe2)
using Steeltoe.Connector.SqlServer;
#endif
#if (DiscoveryEurekaOption)
using Steeltoe.Discovery.Client;
#endif
#if (HostingCloudFoundryOption)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (Steeltoe2ManagementEndpoints)
using Steeltoe.Management.CloudFoundry;
#endif
#if (Steeltoe3ManagementEndpoints)
using Steeltoe.Management.Endpoint;
#endif
#if (AnyTracing)
using Steeltoe.Management.Tracing;
#endif
#if (AnyEfCore)
using Company.WebApplication.CS.Models;
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
#if (Steeltoe2ManagementEndpoints)
            services.AddCloudFoundryActuators(Configuration);
#endif
#if (Steeltoe3ManagementEndpoints)
            services.AddAllActuators(Configuration);
#endif
#if (AnyTracing)
#if (Steeltoe2)
            services.AddDistributedTracing(Configuration);
#elif (Steeltoe30)
            services.AddDistributedTracing();
#else
            services.AddDistributedTracingAspNetCore();
#endif
#endif
            services.AddControllers();
#if (FrameworkNet50)
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company.WebApplication.CS", Version = "v1" });
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if (FrameworkNet50)
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company.WebApplication.CS"));
#endif
            }

#if (DiscoveryEurekaOption)
#if (Steeltoe2 || Steeltoe30)
            app.UseDiscoveryClient();
#endif
#endif
#if (CircuitBreakerHystrixOption)
            app.UseHystrixRequestContext();
#if (Steeltoe2 || Steeltoe30)
            app.UseHystrixMetricsStream();
#endif
#endif
#if (Steeltoe2ManagementEndpoints)
            app.UseCloudFoundryActuators();
#endif
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
#if (Steeltoe3ManagementEndpoints)
                endpoints.MapAllActuators();
#endif
            });
        }
    }
}
