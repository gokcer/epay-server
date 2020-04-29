using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class TransactionItemCategory
    {
        public TransactionItemCategory()
        {
            TransactionItem = new HashSet<TransactionItem>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public string ImageUrl { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string Order { get; set; }

        public virtual ICollection<TransactionItem> TransactionItem { get; set; }
    }
}
