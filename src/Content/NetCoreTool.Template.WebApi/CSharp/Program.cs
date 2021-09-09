using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
#if (Steeltoe2)
using Steeltoe.Extensions.Configuration.PlaceholderCore;
#else
using Steeltoe.Extensions.Configuration.Placeholder;
#endif
#endif
#if (ConfigurationRandomValueOption)
using Steeltoe.Extensions.Configuration.RandomValue;
#endif
#if (DynamicLogging)
using Steeltoe.Extensions.Logging;
#endif

namespace Company.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
#if (ConfigurationPlaceholderOption)
                .AddPlaceholderResolver()
#endif
#if (HostingAzureSpringCloudOption)
                .UseAzureSpringCloudService()
#endif
#if (AnyHosting)
                .UseCloudHosting(8080)
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
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
