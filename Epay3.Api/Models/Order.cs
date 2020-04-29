using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Oid { get; set; }
        public DateTime? Date { get; set; }
        public int? Customer { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Status { get; set; }
        public int? Transaction { get; set; }
        public int? Type { get; set; }
        public int? LocationCard { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual LocationCard LocationCardNavigation { get; set; }
        public virtual Transaction TransactionNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
