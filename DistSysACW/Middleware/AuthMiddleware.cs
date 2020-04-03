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

        public async Task InvokeAsync(HttpContext context, Models.UserContext dbContext, IUserService userService)
        {
            #region Task5
            // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
            //        Then set the correct roles for the User, using claims
            User user = await userService.GetUser(context.Request.Headers["ApiKey"]);
            if (user != null)
            {
                var userIdentity = new ClaimsIdentity();
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                context.User.AddIdentity(userIdentity);
            }
            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
