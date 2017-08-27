using Autofac;
using Jorje.TheWorld.Dal.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Web.Common.Filter
{
    class EnvironmentFilter : IAsyncResourceFilter, IOrderedFilter
    {
        private ILifetimeScope _iLifeTimeScope;

        public EnvironmentFilter(ILifetimeScope iLifetimeScope)
        {
            _iLifeTimeScope = iLifetimeScope;
        }

        public int Order => int.MinValue;

        public async Task OnResourceExecutionAsync(ResourceExecutingContext ctx, ResourceExecutionDelegate next)
        {

            var session = ctx.HttpContext.Request.Headers["session"];
            if (string.IsNullOrWhiteSpace(session))
            {
                session = string.Empty;
            }

            var requestPath = ctx.RouteData.Values["environment"] as string;

            var config = (IConfigurationRoot)ctx.HttpContext.RequestServices.GetService(typeof(IConfigurationRoot));
            //var environmentConfigurations = config.GetEnvironmentConfiguration().ReservationEnvironments;
            //var selectedEnvironmentConfiguration = environmentConfigurations.FirstOrDefault(e => string.Equals(e.Name, requestPath, StringComparison.OrdinalIgnoreCase));
            var builder = new ContainerBuilder();
            var loggerFactory = (ILoggerFactory)ctx.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));

            //builder.Register(c => new WorldDBContext("", loggerFactory))
            //   .As<WorldDBContext>();
        }

        }
}
