using asp_net_auth.Authorization.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// this is an arbitrary example - the intension is that the normal requirement will run
// ensuring the user has the permission will still be taken care of in the has permission
// requirement
//
// this requirement will ensure a random number provided is more than a randomly generated number
// which is supposed to show that we can do extra checks that arent just based on the claim
namespace asp_net_auth.Authorization.Requirements.Permissions.ComplexPermissionEnforcement
{
    public class CanShareDataRequirement : PermissionRequirement
    {
        public CanShareDataRequirement()
        {
            Name = PermissionTypes.ShareData;
        }
    }

    public class CanShareDataRequirementHandler : AuthorizationHandler<CanShareDataRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanShareDataRequirement requirement)
        {
            // hit the database, do something complex
            Random rnd = new Random();
            var value = rnd.Next(1, 10);

            if(value > 5)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
