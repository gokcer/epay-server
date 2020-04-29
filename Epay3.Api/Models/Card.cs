using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Card
    {
        public Card()
        {
            LocationCard = new HashSet<LocationCard>();
            Transaction = new HashSet<Transaction>();
        }

        public int Oid { get; set; }
        public string CardNo { get; set; }
        public bool? Active { get; set; }
        public int? Customer { get; set; }
        public int? MinimumBalanceLimit { get; set; }
        public int? OptimisticLockField { get; set; }
        public bool? Returnable { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual ICollection<LocationCard> LocationCard { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
