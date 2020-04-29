using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class XpobjectType
    {
        public XpobjectType()
        {
            Customer = new HashSet<Customer>();
            PermissionPolicyRole = new HashSet<PermissionPolicyRole>();
            XpweakReferenceObjectTypeNavigation = new HashSet<XpweakReference>();
            XpweakReferenceTargetTypeNavigation = new HashSet<XpweakReference>();
        }

        public int Oid { get; set; }
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<PermissionPolicyRole> PermissionPolicyRole { get; set; }
        public virtual ICollection<XpweakReference> XpweakReferenceObjectTypeNavigation { get; set; }
        public virtual ICollection<XpweakReference> XpweakReferenceTargetTypeNavigation { get; set; }
    }
}
