#if (FrameworkNetCoreApp21)
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#else
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#endif
#if (FrameworkNet50)
using Microsoft.OpenApi.Models;
#endif
#if (Hystrix)
using Steeltoe.CircuitBreaker.Hystrix;
#endif
#if (MySqlConnector && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.MySql;
#endif
#if (MySqlConnector && !Steeltoe2)
using Steeltoe.Connector.MySql;
#endif
#if (RabbitMqConnector && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.RabbitMQ;
#endif
#if (RabbitMqConnector && !Steeltoe2)
using Steeltoe.Connector.RabbitMQ;
#endif
#if (RedisConnector && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.Redis;
#endif
#if (RedisConnector && !Steeltoe2)
using Steeltoe.Connector.Redis;
#endif
#if (SqlServerConnector && Steeltoe2)
using Steeltoe.CloudFoundry.Connector.SqlServer;
#endif
#if (SqlServerConnector && !Steeltoe2)
using Steeltoe.Connector.SqlServer;
#endif
#if (Eureka)
using Steeltoe.Discovery.Client;
#endif
#if (CloudFoundryHosting)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (Steeltoe2ManagementEndpoints)
using Steeltoe.Management.CloudFoundry;
#endif
#if (Steeltoe3ManagementEndpoints)
using Steeltoe.Management.Endpoint;
#endif

namespace Company.WebApplication1
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
#if (CloudFoundryHosting)
            services.ConfigureCloudFoundryOptions(Configuration);
#endif
#if (Eureka)
            services.AddDiscoveryClient(Configuration);
#endif
#if (MySqlConnector)
            services.AddMySqlConnection(Configuration);
#endif
#if (RabbitMqConnector)
            services.AddRabbitMQConnection(Configuration);
#endif
#if (RedisConnector)
            services.AddDistributedRedisCache(Configuration);
#endif
#if (SqlServerConnector)
            services.AddSqlServerConnection(Configuration);
#endif
#if (Hystrix)
            services.AddHystrixCommand<HelloHystrixCommand>("MyCircuitBreakers", Configuration);
            services.AddHystrixMetricsStream(Configuration);
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
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#if (Eureka)
            app.UseDiscoveryClient();
#endif
#if (Hystrix)
            app.UseHystrixRequestContext();
            app.UseHystrixMetricsStream();
#endif
#if (ManagementEndpoints)
            app.UseCloudFoundryActuators();
#endif
            app.UseMvc();
        }
#else
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if (FrameworkNet50)
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company.WebApplication1 v1"));
#endif
            }

#if (Eureka)
            app.UseDiscoveryClient();
#endif
#if (Hystrix)
            app.UseHystrixRequestContext();
            app.UseHystrixMetricsStream();
#endif
#if (Steeltoe2ManagementEndpoints)
            app.UseCloudFoundryActuators();
#endif
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
#endif
    }
}
