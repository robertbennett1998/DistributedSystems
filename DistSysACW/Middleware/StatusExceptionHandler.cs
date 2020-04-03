using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class StatusExceptionHandler
    {
        private readonly RequestDelegate _next;

        public StatusExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (StatusException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                await httpContext.Response.WriteAsync(e.Message);
            }

            return;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class StatusExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseExceptionStatusCodeHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusExceptionHandler>();
        }
    }
}
