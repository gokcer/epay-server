using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class CardGroupCardGroupsCardCards
    {
        public int? Cards { get; set; }
        public int? CardGroups { get; set; }
        public int Oid { get; set; }
        public int? OptimisticLockField { get; set; }

        public virtual CardGroup CardGroupsNavigation { get; set; }
        public virtual Card CardsNavigation { get; set; }
    }
}
