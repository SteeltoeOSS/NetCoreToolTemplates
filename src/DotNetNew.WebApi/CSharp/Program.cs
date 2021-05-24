#if (FrameworkNetCoreApp21)
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#endif
#if (AzureSpringCloudHosting && !FrameworkNetCoreApp21)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (DynamicLogger && FrameworkNetCoreApp21)
using Microsoft.Extensions.Logging;
#endif
#if (CloudFoundryHosting)
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (CloudConfigClient)
using Steeltoe.Extensions.Configuration.ConfigServer;
#endif
#if (PlaceholderConfiguration)
#if (Steeltoe2)
using Steeltoe.Extensions.Configuration.PlaceholderCore;
#else
using Steeltoe.Extensions.Configuration.Placeholder;
#endif
#endif
#if (DynamicLogger)
using Steeltoe.Extensions.Logging;
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
#if (CloudConfigClient)
                .AddConfigServer()
#endif
#if (PlaceholderConfiguration)
                .AddPlaceholderResolver()
#endif
#if (CloudFoundryHosting)
                .UseCloudHosting()
                .AddCloudFoundryConfiguration()
#endif
#if (DynamicLogger)
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
            Host.CreateDefaultBuilder(args)
#if (PlaceholderConfiguration)
                .AddPlaceholderResolver()
#endif
#if (AzureSpringCloudHosting)
                .UseAzureSpringCloudService()
#endif
#if (CloudFoundryHosting)
                .UseCloudHosting()
                .AddCloudFoundryConfiguration()
#endif
#if (CloudConfigClient)
                .AddConfigServer()
#endif
#if (DynamicLogger)
                .ConfigureLogging((context, builder) => builder.AddDynamicConsole())
#endif
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
#endif
    }
}
