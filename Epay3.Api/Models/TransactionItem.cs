using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class TransactionItem
    {
        public TransactionItem()
        {
            ItemPermission = new HashSet<ItemPermission>();
            OrderDetail = new HashSet<OrderDetail>();
            StationItem = new HashSet<StationItem>();
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Type { get; set; }
        public bool? Active { get; set; }
        public bool? System { get; set; }
        public int? ProductType { get; set; }
        public string SpecialCode { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Category { get; set; }
        public byte[] Image { get; set; }

        public virtual TransactionItemCategory CategoryNavigation { get; set; }
        public virtual ICollection<ItemPermission> ItemPermission { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<StationItem> StationItem { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
