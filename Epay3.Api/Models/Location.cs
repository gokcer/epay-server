using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Location
    {
        public Location()
        {
            LocationCard = new HashSet<LocationCard>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public int? Customer { get; set; }
        public bool? Active { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual ICollection<LocationCard> LocationCard { get; set; }
    }
}
