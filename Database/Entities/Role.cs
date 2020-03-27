using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Role : IdentityRole
    {
        public Role()
        { }

        public Role(string roleName) : base(roleName)
        { }

        public Role(string roleName, params Permission[] permissions) : base(roleName)
        {
            var rolePermissions = permissions.Select(p => new RolePermission {
                Role = this,
                PermissionId = p.Id
            });

            if (this.RolePermissions == null)
                this.RolePermissions = new List<RolePermission>();

            this.RolePermissions.AddRange(rolePermissions);
        }

        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
