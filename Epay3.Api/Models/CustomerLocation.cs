using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class CustomerLocation
    {
        public int Oid { get; set; }
        public int? Customer { get; set; }
        public int? Location { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public Customer CustomerNavigation { get; set; }
        public Location LocationNavigation { get; set; }
    }
}
