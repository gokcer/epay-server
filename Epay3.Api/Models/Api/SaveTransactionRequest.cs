using System;

namespace Epay3.Api.Models.Api
{
    public class SaveTransactionRequest
    {
        public string CardNo { get; set; }
        public decimal Amount { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public int Cycle { get; set; }
        public DateTime Date { get; set; }
        public string TransactionToken { get; set; }
    }
}