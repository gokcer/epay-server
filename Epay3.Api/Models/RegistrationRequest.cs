using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class RegistrationRequest
    {
        public int Oid { get; set; }
        public string VerificationCode { get; set; }
        public DateTime? DateSent { get; set; }
        public string IpAddress { get; set; }
        public DateTime? DateConsumed { get; set; }
        public int? Customer { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
    }
}
