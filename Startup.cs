using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // middleware runs in the order specified here. Order matters.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Only if you're using html directly, not cshtml
            app.UseDefaultFiles();  // automatically bind default files

            // ASP.NET core doesn't serve static files by default, it must be configured
            // This makes sense because API projects which are very common nowdays don't serve statics, or at least they ought not to
            app.UseStaticFiles();

            /*
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            */

        }
    }
}
