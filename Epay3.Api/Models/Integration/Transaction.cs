using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Integration
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateEffective { get; internal set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Total { get; set; }
        public int? Type { get; set; }
        public int? ProductType { get; set; }
        public string CardNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int? EmployeeId { get; set; }
        public string Employee { get; set; }
        public int IdTransactionDetail { get; internal set; }
        public bool? IsCancel { get; set; }
    }
}
