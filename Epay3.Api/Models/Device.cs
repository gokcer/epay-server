using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Device
    {
        public Device()
        {
            DeviceLogin = new HashSet<DeviceLogin>();
            Transaction = new HashSet<Transaction>();
        }

        public int Oid { get; set; }
        public string DeviceToken { get; set; }
        public string Serial { get; set; }
        public int? DeviceMode { get; set; }
        public int? ProductType { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? PollPeriod { get; set; }
        public bool? MaskOrderCustomer { get; set; }
        public bool? KioskMode { get; set; }
        public int? CompletedOrderKeepDuration { get; set; }

        public virtual ICollection<DeviceLogin> DeviceLogin { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
