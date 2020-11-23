using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace WebAppExample.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder UseMiddleWareAsEndpoint<T>(this IEndpointRouteBuilder endpoints,
            string path)
        {
            var name = "my name is radwan ";

            var app = endpoints.CreateApplicationBuilder();
            app.UseMiddleware<T>();
            var requestDelegate = app.Build();
            return endpoints.MapGet(path, requestDelegate);
        }
    }
}