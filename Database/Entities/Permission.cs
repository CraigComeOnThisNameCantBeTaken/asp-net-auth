using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Permission
    {
        // an easy contant to map to like ReadVariants
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
