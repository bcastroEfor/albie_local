using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Albie.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var v = ActioBP.General.Utility.NetVersion.GetNetCoreVersion();
            System.Console.WriteLine(v);

            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args)
                //.UseKestrel()
                .ConfigureKestrel(options => { options.AddServerHeader = false; })
                .UseUrls("http://0.0.0.0:5001")
                .UseStartup<Startup>();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == EnvironmentName.Development;

            if (!isDevelopment)
            {
                hostBuilder = hostBuilder.ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                });
            }

            return hostBuilder;
        }
    }
}
