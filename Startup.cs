using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebAppExample.Models;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAppExample.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebAppExample
{
    public class Startup
    {

        private readonly IConfiguration Configuration;
        private IWebHostEnvironment Environment;
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Configuration = config;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:ProductConnection"]);

                if (Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(true);
                }
            }, ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DataContext dbContext,
            IHostApplicationLifetime lifetime)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                bool runSeedDataBase;
                if (bool.TryParse(Configuration["INITDB"], out runSeedDataBase) &&
                     runSeedDataBase)
                {
                    SeedData.SeedDataBase(dbContext);
                    lifetime.StopApplication();
                }
            }

            app.UseMiddleware<TestMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!\n");
                });

                endpoints.UseWebServiceEndPoint();
            });


        }
    }
}
