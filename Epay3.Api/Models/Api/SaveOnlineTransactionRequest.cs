using System.Collections.Generic;

namespace Epay3.Api.Models.Api
{
    public class SaveOnlineTransactionRequest
    {
        public string CardNo { get; set; }

        public int OrderType { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public int? LocationCard { get; set; }
    }
}