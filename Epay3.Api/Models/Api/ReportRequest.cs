using System;

namespace Epay3.Api.Models.Api
{
    public class ReportRequest
    {
        public bool Sales { get; set; } = false;
        public bool Charges { get; set; } = false;
        public bool Orders { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int ProductType { get; set; } = 0;

    }
}