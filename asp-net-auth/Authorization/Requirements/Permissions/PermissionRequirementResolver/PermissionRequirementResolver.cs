using asp_net_auth.Authorization.Requirements.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace asp_net_auth.Authorization.Permissions.PermissionRequirementResolver
{
    public class PermissionRequirementResolver : IPermissionRequirementResolver
    {
        private readonly IEnumerable<PermissionRequirement> permissionRequirements;

        public PermissionRequirementResolver(IEnumerable<PermissionRequirement> permissionRequirements)
        {
            this.permissionRequirements = permissionRequirements;
        }

        public IEnumerable<IAuthorizationRequirement> Resolve(string permissionName)
        {
            return permissionRequirements.Where(r => r.Name == permissionName);
        }
    }
}
