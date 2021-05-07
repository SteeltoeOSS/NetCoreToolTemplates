#if FRAMEWORK_NET_CORE_APP_21
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#endif

namespace Steeltoe.WebApi.CSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if FRAMEWORK_NET_CORE_APP_21
            CreateWebHostBuilder(args).Build().Run();
#else
            CreateHostBuilder(args).Build().Run();
#endif
        }

#if FRAMEWORK_NET_CORE_APP_21
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
            return builder;
        }
#else
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
#endif
    }
}
