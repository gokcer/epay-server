using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class ItemPermission
    {
        public int Oid { get; set; }
        public int? Customer { get; set; }
        public int? TransactionItem { get; set; }
        public bool? Permission { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public bool? Active { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual TransactionItem TransactionItemNavigation { get; set; }
    }
}
