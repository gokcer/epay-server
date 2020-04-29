using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class StationItem
    {
        public int Oid { get; set; }
        public int? Station { get; set; }
        public int? TransactionItem { get; set; }
        public int? PreparationTime { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public virtual Station StationNavigation { get; set; }
        public virtual TransactionItem TransactionItemNavigation { get; set; }
    }
}
