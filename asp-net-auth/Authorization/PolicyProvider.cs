using asp_net_auth.Authorization.Constants;
using asp_net_auth.Authorization.Permissions.PermissionRequirementResolver;
using asp_net_auth.Authorization.Requirements;
using asp_net_auth.Authorization.Requirements.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization
{
    public class PolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IPermissionRequirementResolver permissionRequirementResolver;

        public PolicyProvider(
            IPermissionRequirementResolver permissionRequirementResolver,
            IOptions<AuthorizationOptions> options) : base(options)
        {
            this.permissionRequirementResolver = permissionRequirementResolver;
        }

        // todo: get rid of . notation. permissions cant have values so it doesnt make sense
        // and we cant pass a resource from there because the purpose of a resource is to be set
        // at runtime
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // instead of Permission.name as a delimiter for string.StartsWith() check
            // we are just using a permission name and checking against a list
            // of all available permissions.
            // this is because using the autorization service requires a policy and
            // we dont want consumers to have to know to string concatenate Permission.{permissionName}
            if (PermissionTypes.List.Any(t => t == policyName))
            {
                var permissionName = policyName;

                var permissionRequirements = permissionRequirementResolver
                    .Resolve(permissionName)
                    .ToList();
                permissionRequirements.Add(new HasPermissionRequirement(permissionName));
                
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(permissionRequirements.ToArray())
                    .Build();
                return Task.FromResult(policy);
            }

            return base.GetPolicyAsync(policyName);
        }
    }
}
