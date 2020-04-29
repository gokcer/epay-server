using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class OrderDetail
    {
        public int Oid { get; set; }
        public int? Order { get; set; }
        public int? TransactionItem { get; set; }
        public int? TransactionDetail { get; set; }
        public decimal? Quantity { get; set; }
        public int? Station { get; set; }
        public int? Status { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? CancelTransactionDetail { get; set; }

        public virtual TransactionDetail CancelTransactionDetailNavigation { get; set; }
        public virtual Order OrderNavigation { get; set; }
        public virtual Station StationNavigation { get; set; }
        public virtual TransactionDetail TransactionDetailNavigation { get; set; }
        public virtual TransactionItem TransactionItemNavigation { get; set; }
    }
}
