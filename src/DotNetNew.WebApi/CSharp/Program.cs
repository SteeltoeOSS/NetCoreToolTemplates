#if (FrameworkNetCoreApp21)
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#endif
#if (AzureSpringCloud)
using Microsoft.Azure.SpringCloud.Client;
#endif
#if (PlaceholderConfiguration)
using Steeltoe.Extensions.Configuration.Placeholder;
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
                .UseStartup<Startup>();
            return builder;
        }
#else
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
#if (PlaceholderConfiguration)
                .AddPlaceholderResolver()
#endif
#if (AzureSpringCloud)
                .UseAzureSpringCloudService()
#endif
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
#endif
    }
}
