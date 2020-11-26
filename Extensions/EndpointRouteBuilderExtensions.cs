using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using WebAppExample.Models;

namespace WebAppExample.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder UseMiddleWareAsEndpoint<T>(this IEndpointRouteBuilder endpoints,
            string path)
        {
            var app = endpoints.CreateApplicationBuilder();
            app.UseMiddleware<T>();
            var requestDelegate = app.Build();

            return endpoints.Map(path, requestDelegate);
        }


        public static IEndpointConventionBuilder UseCustom(this IEndpointRouteBuilder endpoints, string path)
        {
            var pattern = RoutePatternFactory.Parse(path);

            var endpointBuilder = new RouteEndpointBuilder(
                context =>
                {
                    context.Response.WriteAsync("This is from a custom Endpoint");
                    return Task.CompletedTask;
                },
                pattern,
                0
            );


            var customDataSource = endpoints.DataSources.OfType<CustomEndpointDataSource>()
                .FirstOrDefault();
            if (customDataSource == null)
            {
                customDataSource = new CustomEndpointDataSource();

                endpoints.DataSources.Add(customDataSource);
            }

            return customDataSource.AddEndpoint(endpointBuilder);
        }
    }

    // TODO: Create separate files for classes
    public class CustomEndpointDataSource : EndpointDataSource
    {
        private List<CustomEndpointConventionBuilder> _endpointConventionBuilders;

        public CustomEndpointDataSource()
        {
            _endpointConventionBuilders = new List<CustomEndpointConventionBuilder>();
        }

        public override IChangeToken GetChangeToken()
        {
            return NullChangeToken.Singleton;
        }

        public IEndpointConventionBuilder AddEndpoint(EndpointBuilder endpointBuilder)
        {
            var builder = new CustomEndpointConventionBuilder(endpointBuilder);
            _endpointConventionBuilders.Add(builder);

            return builder;
        }

        public override IReadOnlyList<Endpoint> Endpoints =>
            _endpointConventionBuilders.Select(e => e.Build())
                .ToList();
    }

    public class CustomEndpointConventionBuilder : IEndpointConventionBuilder
    {
        EndpointBuilder EndpointBuilder { get; }

        private readonly List<Action<EndpointBuilder>> _conventions;

        public CustomEndpointConventionBuilder(EndpointBuilder endpointBuilder)
        {
            EndpointBuilder = endpointBuilder;
            _conventions = new List<Action<EndpointBuilder>>();
        }

        public void Add(Action<EndpointBuilder> convention)
        {
            _conventions.Add(convention);
        }

        public Endpoint Build()
        {
            foreach (var convention in _conventions)
            {
                convention(EndpointBuilder);
            }

            return EndpointBuilder.Build();
        }
    }
}