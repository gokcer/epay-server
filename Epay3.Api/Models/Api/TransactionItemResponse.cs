using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Api
{

    public class TransactionItemResponse
    {
        public int IdTransactionItem { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductType { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public byte[] Image { get; set; }
    }
}
