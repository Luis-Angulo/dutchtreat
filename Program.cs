using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DutchTreat
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hb = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Setup)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
            return hb;
        }

        private static void Setup(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            // Last config wins, the second arg is whether the file is optional
            builder.AddJsonFile("D:\\vsprojects\\DutchTreat\\config.json", false, true)
                .AddXmlFile("config.xml", true)
                .AddEnvironmentVariables();
        }
    }
}
