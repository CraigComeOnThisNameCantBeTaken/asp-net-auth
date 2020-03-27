using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Requirements
{
    public class AdminRequirement : IAuthorizationRequirement
    {
    }

    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var roleClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if(roleClaim.Value == "adminLevel")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
