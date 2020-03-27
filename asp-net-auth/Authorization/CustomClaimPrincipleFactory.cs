using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization
{
    //todo: multiple database hits happening here
    // todo: known issue. claims being added where type is permission name instead of value
    public class CustomClaimPrincipleFactory : UserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly DataContext dataContext;

        public CustomClaimPrincipleFactory(UserManager<IdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor,
            DataContext dataContext) : base(userManager, optionsAccessor)
        {
            this.dataContext = dataContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var roles = await UserManager.GetRolesAsync(user);
            var permissions = await dataContext.AspNetPermissions
                    .Include(p => p.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .Where(p => p.RolePermissions.Any(rp => roles.Contains(rp.Role.Name)))
                    .ToListAsync();

            foreach (var permission in permissions)
            {
                identity.AddClaim(new Claim(permission.Name, "true"));
            }

            return identity;
        }
    }
}
