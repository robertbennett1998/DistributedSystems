using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Middleware
{
    public class StatusExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            try
            {
                await _next(httpContext);
            }
            catch (StatusException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                await httpContext.Response.WriteAsync(e.Message);
            }
        }
    }

    public static class StatusExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseStatusExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusExceptionHandlerMiddleware>();
        }
    }
}
