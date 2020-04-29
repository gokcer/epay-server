using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Integration
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNo { get; set; }
        public decimal? Balance { get; set; }
        public bool? Active { get; set; }
    }
}
