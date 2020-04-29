using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Station
    {
        public Station()
        {
            OrderDetail = new HashSet<OrderDetail>();
            StationItem = new HashSet<StationItem>();
        }

        public int Oid { get; set; }
        public int? Employee { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public string Name { get; set; }

        public virtual Customer EmployeeNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<StationItem> StationItem { get; set; }
    }
}
