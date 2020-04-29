using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Integration
{
    public class StaffResponse
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public IEnumerable<Card> Cards { get; set; }
        public String Phone { get; set; }
        public bool Employee { get; set; }
        public string CitizenshipNumber { get; set; }
        public string Email { get; set; }
        public string NotificationToken { get; set; }
        public string Team { get; set; }
    }
}
