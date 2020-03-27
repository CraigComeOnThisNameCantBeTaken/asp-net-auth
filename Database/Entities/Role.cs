using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Role : IdentityRole
    {
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
