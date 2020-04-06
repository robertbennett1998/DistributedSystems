using DistSysACW.Exceptions;
using DistSysACW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACW.Filters
{
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();

            if (authAttribute != null)
            {
                string[] roles = authAttribute.Roles.Split(',');
                foreach (string role in roles)
                {
                    if (context.HttpContext.User.IsInRole(role))
                    {
                        return;
                    }
                }

                if (roles.All(s => s == User.Role.Admin.ToString()))
                    throw new UserRoleException("Unauthorized. Admin access only.");

                throw new UserRoleException("Unauthorized. Check ApiKey in Header is correct.");
            }
        }
    }
}
