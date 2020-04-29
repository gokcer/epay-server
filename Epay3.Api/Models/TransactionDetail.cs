using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class TransactionDetail
    {
        public TransactionDetail()
        {
            OrderDetailCancelTransactionDetailNavigation = new HashSet<OrderDetail>();
            OrderDetailTransactionDetailNavigation = new HashSet<OrderDetail>();
        }

        public int Oid { get; set; }
        public int? Transaction { get; set; }
        public int? Item { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Total { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? OptimisticLockField { get; set; }
        public bool? IsCancel { get; set; }

        public virtual TransactionItem ItemNavigation { get; set; }
        public virtual Transaction TransactionNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetailCancelTransactionDetailNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetailTransactionDetailNavigation { get; set; }
    }
}
