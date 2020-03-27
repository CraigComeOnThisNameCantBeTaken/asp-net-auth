using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_auth.Authorization.Constants
{
    public static class PermissionTypes
    {
        public const string EditData = "editData";
        public const string ShareData = "shareData";

        public static IReadOnlyCollection<string> List = new List<string>
        {
            EditData,
            ShareData
        }.AsReadOnly();
    }
}
