using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class DeviceRegistration
    {
        public int Oid { get; set; }
        public DateTime? Date { get; set; }
        public string SourceAddress { get; set; }
        public bool? Success { get; set; }
        public string Serial { get; set; }
        public string Token { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
    }
}
