using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Requirements.Permissions
{
    public class HasPermissionRequirement : OperationAuthorizationRequirement
    {
        public HasPermissionRequirement(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Value { get; }
    }

    public class HasPermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionRequirement requirement)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.Name);

            if (claim == null ||
                !string.IsNullOrEmpty(requirement.Value) && claim.Value != requirement.Value)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

