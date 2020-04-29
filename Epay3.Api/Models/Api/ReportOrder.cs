using System;
using System.Collections.Generic;

namespace Epay3.Api.Models.Api
{
    public class ReportOrder
    {
        public int Oid { get; set; }
        public string CustomerName { get; set; }
        public DateTime? Date { get; set; }
        public List<ReportOrderItem> Items { get; set; }
        public int? Status { get; set; }

        public bool SingleItem { get; set; }
        public int? Type { get; set; }
        public string Location { get; set; }
    }
}