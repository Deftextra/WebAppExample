using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebAppExample.Models;
using WebAppExample.Services;

namespace WebAppExample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _configuration = config;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Counter>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy.Test, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader();

                });
            });
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(_configuration["ConnectionStrings:ProductConnection"]);

                if (_environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(true);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DataContext context,
            IHostApplicationLifetime lifetime)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (bool.TryParse(_configuration["INITDB"], out var runSeedDataBase) &&
                    runSeedDataBase)
                {
                    SeedData.SeedDataBase(context);
                    lifetime.StopApplication();
                }
            }

            // Add this point we match a http context to a route.
            app.UseRouting();

            // Here to http request is routed to the correct endpoint.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}