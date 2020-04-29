using System;
using System.Collections.Generic;

namespace Epay3.Api.Models.Api
{
    public class ReportResponse
    {
        public Decimal? TotalCharges { get; set; }
        public Decimal? TotalSales { get; set; }
        public int? CountCharges { get; set; }
        public int? CountSales { get; set; }
        public int? CountCustomer { get; set; }

        public IEnumerable<ReportTransaction> Charges { get; set; }
        public IEnumerable<ReportTransaction> Sales { get; set; }
        public IEnumerable<ReportOrder> Orders { get; set; }
    }
}