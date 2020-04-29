using System;

namespace Epay3.Api.Models.Api
{
    public class CustomerReportOrder
    {
        public int? Status { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? Date { get; set; }
        public int IdItem { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public byte[] Image { get; set; }
    }
}