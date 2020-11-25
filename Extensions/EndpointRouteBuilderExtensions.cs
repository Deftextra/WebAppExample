using System.Net;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace WebAppExample.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder UseMiddleWareAsEndpoint<T>(this IEndpointRouteBuilder endpoints, string path)
        {

            var app = endpoints.CreateApplicationBuilder();

            app.UseMiddleware<T>();

            var requestDelegate = app.Build();
            
            

            return endpoints.MapGet(path, requestDelegate);
        }
        
    }
}