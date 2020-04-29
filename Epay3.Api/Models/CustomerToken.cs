using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models
{
    public class CustomerToken
    {
        public string Client { get; set; }
        public string NotificationToken { get; set; }
        public string Phone { get; set; }
    }
}
