using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailService, NullMailService>();
            services.AddControllersWithViews();
            // must be added here unlike tutorial says
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // middleware runs in the order specified here. Order matters.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/error");
            }
            app
            .UseStaticFiles()
            // shortcut from the course, in a real app we should build the npm production dependencies we need, put them in a dist or src folder and serve those as statics
            .UseNodeModules()
            .UseRouting()
            .UseEndpoints(config =>
            {                
                config.MapControllerRoute("Fallback",
                // pattern for finding controllers and methods by convention
                "{controller}/{action}/{id?}",
                // default if no match is found
                new { controller = "App", action = "Index" });
                config.MapRazorPages();
            });
        }
    }
}
