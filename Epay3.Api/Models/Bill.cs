using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Bill
    {
        public int Oid { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? OptimisticLockField { get; set; }
    }
}
