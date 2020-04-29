using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class LocationCard
    {
        public LocationCard()
        {
            Order = new HashSet<Order>();
        }

        public int Oid { get; set; }
        public int? Card { get; set; }
        public int? Location { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public int? DeviceLogin { get; set; }
        public int? OpenedBy { get; set; }

        public virtual Card CardNavigation { get; set; }
        public virtual DeviceLogin DeviceLoginNavigation { get; set; }
        public virtual Location LocationNavigation { get; set; }
        public virtual Customer OpenedByNavigation { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
