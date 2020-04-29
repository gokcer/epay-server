using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Transaction
    {
        public Transaction()
        {
            Order = new HashSet<Order>();
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        public int Oid { get; set; }
        public DateTime? Date { get; set; }
        public int? Card { get; set; }
        public decimal? Amount { get; set; }
        public decimal? OldBalance { get; set; }
        public decimal? NewBalance { get; set; }
        public int? Cycle { get; set; }
        public int? DeviceLogin { get; set; }
        public int? Device { get; set; }
        public int? Employee { get; set; }
        public string SourceAddress { get; set; }
        public decimal? TotalCharges { get; set; }
        public decimal? TotalSales { get; set; }
        public int? ProductType { get; set; }
        public int? OptimisticLockField { get; set; }
        public DateTime? ProvisionDate { get; set; }
        public bool? IsCancel { get; set; }
        public DateTime? DateEffective { get; set; }

        public virtual Card CardNavigation { get; set; }
        public virtual DeviceLogin DeviceLoginNavigation { get; set; }
        public virtual Device DeviceNavigation { get; set; }
        public virtual Customer EmployeeNavigation { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
