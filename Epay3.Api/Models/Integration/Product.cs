using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Integration
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool? Active { get; set; }
        public int? ProductType { get; set; }
        public int? Type { get; set; }
    }
}
