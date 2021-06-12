using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ClientAngular
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
            {
                var env = webHostBuilderContext.HostingEnvironment;
                configurationbuilder.SetBasePath(env.ContentRootPath);
                configurationbuilder.AddJsonFile("appsettings.json", false, true);
                configurationbuilder.AddEnvironmentVariables();
            })
            .UseStartup<Startup>();
    }
}
