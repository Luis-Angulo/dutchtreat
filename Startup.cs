using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AutoMapper;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using DutchTreat.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            // port: 5000
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DutchContext>();

            services.AddAuthentication().AddCookie().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidAudience = _config["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                };
            });

            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            // This was a PITA. TL;DR: if you're using .net < 5.x upgrate to 5.x, then
            // add NewtonsoftJson to the packages in the project, then add the config as shown here.
            services.AddMvc().AddNewtonsoftJson(
                o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );
            services.AddDbContext<DutchContext>(builder =>
            {
                builder.UseSqlServer(_config.GetConnectionString("DutchConnString"));
            });
            // Automapper, ew
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // middleware runs in the order specified here. Order matters.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app
            .UseStaticFiles()
            // shortcut from the course, in a real app we should build the npm production dependencies we need, put them in a dist or src folder and serve those as statics
            .UseNodeModules()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
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
