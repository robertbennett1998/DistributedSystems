using DistSysACW.Models;
using DistSysACW.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DistSysACW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            User user = await userService.GetUserByApiKey(context.Request.Headers["ApiKey"]);
            if (user != null)
            {
                var userIdentity = new ClaimsIdentity();
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                context.User.AddIdentity(userIdentity);
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
