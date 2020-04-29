using System;

namespace Epay3.Api.Models.Api
{
    public class CustomerReportTransactionDetail
    {
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public decimal? Total { get; set; }
        public decimal? Quantity { get; set; }
    }
}