using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
            var hb = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
            return hb;
        }


    }
}
