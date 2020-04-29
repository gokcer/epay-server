using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class CardGroup
    {
        public CardGroup()
        {
            CardGroupCardGroupsCardCards = new HashSet<CardGroupCardGroupsCardCards>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
        public int? ObjectType { get; set; }
        public string Amount { get; set; }

        public virtual XpobjectType ObjectTypeNavigation { get; set; }
        public virtual ICollection<CardGroupCardGroupsCardCards> CardGroupCardGroupsCardCards { get; set; }
    }
}
