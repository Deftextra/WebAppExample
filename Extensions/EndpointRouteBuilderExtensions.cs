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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.IO;

namespace WebAppExample.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        private const string BaseAddress = "/api/products";

        public static IEndpointConventionBuilder UseMiddleWareAsEndpoint<T>(this IEndpointRouteBuilder endpoints, string path)
        {

            var app = endpoints.CreateApplicationBuilder();

            app.UseMiddleware<T>();

            var requestDelegate = app.Build();

            return endpoints.MapGet(path, requestDelegate);
        }

        public static void UseWebServiceEndPoint(this IEndpointRouteBuilder endpoints)
        {

            endpoints.MapGet($"{BaseAddress}/{{id:int}}", async context =>
            {
                using var dbContext = context.RequestServices.GetService<DataContext>();

                var productId = long.Parse(context.Request.RouteValues["id"] as string);

                var product = dbContext.Products
                .Find(productId);

                if (product == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(product));
            });

            endpoints.MapGet(BaseAddress, async context =>
            {
                using var dbContext = context.RequestServices.GetService<DataContext>();
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(dbContext.Products));
            });

            endpoints.MapPost(BaseAddress, async context =>
            {
                // Will not do checks. We have to do this manually.
                Product product = await JsonSerializer.DeserializeAsync<Product>(context.Request.Body);

                using var dbContext = context.RequestServices.GetService<DataContext>();
                // This will add it to the entiy.
                await dbContext.AddAsync(product);
                // Reflect this new change in the DB.
                await dbContext.SaveChangesAsync();

                context.Response.StatusCode = StatusCodes.Status200OK;
            });


        }

    }
}