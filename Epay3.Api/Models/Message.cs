using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Message
    {
        public int Oid { get; set; }
        public int? To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public virtual Customer ToNavigation { get; set; }
    }
}
