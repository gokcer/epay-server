using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class DeviceLogin
    {
        public DeviceLogin()
        {
            LocationCard = new HashSet<LocationCard>();
            Transaction = new HashSet<Transaction>();
        }

        public int Oid { get; set; }
        public DateTime? Date { get; set; }
        public string Serial { get; set; }
        public string SourceAddress { get; set; }
        public string UserNameReceived { get; set; }
        public string PasswordReceived { get; set; }
        public string Success { get; set; }
        public int? Employee { get; set; }
        public int? Device { get; set; }
        public string Token { get; set; }
        public int? OptimisticLockField { get; set; }

        public virtual Device DeviceNavigation { get; set; }
        public virtual Customer EmployeeNavigation { get; set; }
        public virtual ICollection<LocationCard> LocationCard { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
