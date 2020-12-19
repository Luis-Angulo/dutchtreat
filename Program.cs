using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;

namespace DutchTreat
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            SeedDb(host);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseStartup<Startup>()
            .Build();

        private static void SeedDb(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed();
            }
        }

        private static void SetupConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            // Last config wins, the second arg is whether the file is optional
            builder.AddJsonFile("D:\\vsprojects\\DutchTreat\\config.json", false, true)
                .AddXmlFile("config.xml", true)
                .AddEnvironmentVariables();
        }
    }
}
