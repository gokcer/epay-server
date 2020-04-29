using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class PermissionPolicyUser
    {
        public PermissionPolicyUser()
        {
            PermissionPolicyUserUsersPermissionPolicyRoleRoles = new HashSet<PermissionPolicyUserUsersPermissionPolicyRoleRoles>();
        }

        public Guid Oid { get; set; }
        public string StoredPassword { get; set; }
        public bool? ChangePasswordOnFirstLogon { get; set; }
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public virtual ICollection<PermissionPolicyUserUsersPermissionPolicyRoleRoles> PermissionPolicyUserUsersPermissionPolicyRoleRoles { get; set; }
    }
}
