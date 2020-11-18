using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebAppExample.Models;

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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DataContext context,
            IHostApplicationLifetime lifetime)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                bool runSeedDataBase;
                if (bool.TryParse(Configuration["INITDB"], out runSeedDataBase) &&
                     runSeedDataBase)
                {
                    SeedData.SeedDataBase(context);
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
                    await context.Response.WriteAsync(Environment.IsDevelopment().ToString());
                });
            });


        }
    }
}
