#if (FrameworkNetCoreApp21)
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#endif
#if (AzureSpringCloudHostingOption && !FrameworkNetCoreApp21)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (DynamicLoggerOption && FrameworkNetCoreApp21)
using Microsoft.Extensions.Logging;
#endif
#if (CloudFoundryHostingOption)
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (CloudConfigOption)
using Steeltoe.Extensions.Configuration.ConfigServer;
#endif
#if (PlaceholderOption)
#if (Steeltoe2)
using Steeltoe.Extensions.Configuration.PlaceholderCore;
#else
using Steeltoe.Extensions.Configuration.Placeholder;
#endif
#endif
#if (DynamicLoggerOption)
using Steeltoe.Extensions.Logging;
#endif
#if (MessagingRabbitMqOption)
using Steeltoe.Messaging.RabbitMQ.Host;
#endif

namespace Company.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if (FrameworkNetCoreApp21)
            CreateWebHostBuilder(args).Build().Run();
#else
            CreateHostBuilder(args).Build().Run();
#endif
        }

#if (FrameworkNetCoreApp21)
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
#if (CloudConfigOption)
                .AddConfigServer()
#endif
#if (PlaceholderOption)
                .AddPlaceholderResolver()
#endif
#if (CloudFoundryHostingOption)
                .UseCloudHosting()
                .AddCloudFoundryConfiguration()
#endif
#if (DynamicLoggerOption)
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddDynamicConsole();
                })
#endif
                .UseStartup<Startup>();
            return builder;
        }
#else
        public static IHostBuilder CreateHostBuilder(string[] args) =>
#if (MessagingRabbitMqOption)
            RabbitHost.CreateDefaultBuilder()
#else
            Host.CreateDefaultBuilder(args)
#endif
#if (PlaceholderOption)
                .AddPlaceholderResolver()
#endif
#if (AzureSpringCloudHostingOption)
                .UseAzureSpringCloudService()
#endif
#if (CloudFoundryHostingOption)
                .UseCloudHosting()
                .AddCloudFoundryConfiguration()
#endif
#if (CloudConfigOption)
                .AddConfigServer()
#endif
#if (DynamicLoggerOption)
                .ConfigureLogging((context, builder) => builder.AddDynamicConsole())
#endif
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
#endif
    }
}
