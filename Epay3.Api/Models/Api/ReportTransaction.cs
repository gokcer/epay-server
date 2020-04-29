using System;

namespace Epay3.Api.Models.Api
{
    public class ReportTransaction
    {
        public string CardNo { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime? Date { get; set; }
    }
}