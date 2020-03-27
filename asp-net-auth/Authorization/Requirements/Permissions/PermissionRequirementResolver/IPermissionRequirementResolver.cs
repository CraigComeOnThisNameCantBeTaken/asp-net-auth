using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Permissions.PermissionRequirementResolver
{
    public interface IPermissionRequirementResolver
    {
        IEnumerable<IAuthorizationRequirement> Resolve(string permissionName);
    }
}
