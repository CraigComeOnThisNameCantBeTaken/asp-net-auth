using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Attributes
{
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Setup required permissions
        /// </summary>
        /// <param name="commaSeperatedPermissions">Permission identifiers seperated by commas</param>
        /// <returns></returns>
        public RequiresPermissionAttribute(string commaSeperatedPermissions)
            : base(typeof(RequiresPermissionFilter))
        {
            Arguments = commaSeperatedPermissions.Split(",");
        }
    }

    internal class RequiresPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] permissionIdentifiers;
        private readonly IAuthorizationService authorizationService;

        public RequiresPermissionFilter(string[] permissionIdentifiers,
            IAuthorizationService authorizationService)
        {
            this.permissionIdentifiers = permissionIdentifiers;
            this.authorizationService = authorizationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            foreach (var permissionIdentifier in permissionIdentifiers)
            {
                var policyName = $"Permission.{permissionIdentifier}";
                var authorized = await authorizationService.AuthorizeAsync(context.HttpContext.User, policyName);

                if (!authorized.Succeeded)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}
