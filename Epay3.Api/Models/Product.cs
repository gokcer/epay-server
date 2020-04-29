using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Product
    {
        public Product()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Type { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }

        public ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
