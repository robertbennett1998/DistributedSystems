using DistSysACW.Attributes;
using DistSysACW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DistSysACW.Middleware
{
    public class LogAuthorisedRequestsMiddleware
    {
        private readonly RequestDelegate _next;

        public LogAuthorisedRequestsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILoggingService loggingService)
        {
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;

            if (endpoint != null && endpoint.Metadata.Any(m => m.GetType() == typeof(AuthorizeAttribute)))
            {
                await loggingService.LogAuthorisedRequest(httpContext.Request.Headers["ApiKey"], httpContext.User.FindFirst(ClaimTypes.Name)?.Value, httpContext.User.FindFirst(ClaimTypes.Role)?.Value, httpContext.Request.Method, httpContext.Request.Path.ToString());
            }

            await _next(httpContext);
        }
    }

    public static class LogAuthorisedRequestsMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogAuthorisedRequestsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogAuthorisedRequestsMiddleware>();
        }
    }
}
