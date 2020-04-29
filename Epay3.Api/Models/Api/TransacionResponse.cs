using System;
using System.Collections.Generic;

namespace Epay3.Api.Models.Api
{
    public class TransacionResponse
    {
        public DateTime? DateTime { get; set; }
        public decimal? Total { get; set; }
        public decimal? OldBalance { get; set; }
        public decimal? NewBalance { get; set; }

        public IList<TransactionDetailResponse> TransactionDetails { get; set; }

    }
}