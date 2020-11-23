using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace WebAppExample.Attributes
{
    // Note: Make sure to get ActionMethodSelectorAttribute from the Microsoft.AspNetCore.Mvc.ActionConstraints;
    // Namespace and not System.Web.Routing namespace which is not compatible with new aps.net core versions
    public class NamespaceConstraint : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            foreach (var x in routeContext.RouteData.Values)
            {
                Console.WriteLine(x);
            }

            foreach (var x in routeContext.RouteData.DataTokens)
            {
                Console.WriteLine(x);
            }
            
            return true;
        }
    }
}