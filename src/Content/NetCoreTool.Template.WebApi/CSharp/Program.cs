#if StreamRabbitMqOption
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace Company.WebApplication1
{
    [EnableBinding(typeof(IProcessor))]
    public class Program
    {
        static async Task Main(string[] args)
        {
            await StreamHost.CreateDefaultBuilder<Program>(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddDebug();
                        builder.AddConsole();
                    });
                }).RunConsoleAsync();
        }

        [StreamListener(ISink.INPUT)]
        [SendTo(ISource.OUTPUT)]
        public string Handle(string inputVal)
        {
            return inputVal.ToUpper();
        }
    }
}
#else
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
#if (DynamicLogging && FrameworkNetCoreApp21)
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
#if (DynamicLogging)
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
#if (DynamicLogging)
                .ConfigureLogging((context, builder) => builder.AddDynamicConsole())
#endif
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
#endif
    }
}
#endif
