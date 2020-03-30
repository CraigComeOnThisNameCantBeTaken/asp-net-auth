using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Requirements.Permissions
{
    public class HasPermissionRequirement : OperationAuthorizationRequirement
    {
        public HasPermissionRequirement(string name)
        {
            Name = name;
        }
    }

    public class HasPermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionRequirement requirement)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == "Permission");

            if (claim != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

