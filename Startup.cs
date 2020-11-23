using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using WebAppExample.Models;
using WebAppExample.Models.ValueProviders;

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
            services.AddDbContext<DataContext>(options =>

            {
                options.UseSqlServer(_configuration["ConnectionStrings:ProductConnection"]);

                if (_environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(true);
                }
            });

            services.AddControllers(opts => { opts.ValueProviderFactories.Add(new CustomValueProviderFactory()); });
        }

        // This method gets called by the runtime. Use this methodcc to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DataContext dbContext,
            IHostApplicationLifetime lifetime)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (bool.TryParse(_configuration["INITDB"], out var runSeedDataBase) &&
                    runSeedDataBase)
                {
                    SeedData.SeedDataBase(dbContext);
                    lifetime.StopApplication();
                }
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Blog",
                    pattern: "/blog/{*article}",
                    defaults: new {controller = "Blog", action = "Article"}
               );
                    
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}",
                    constraints: null,
                    dataTokens: new { Namespace = "Testing", hello = "helloall" }
                );
            });
        }
    }
}