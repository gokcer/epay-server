using System;

namespace Epay3.Api.Models.Api
{
    public class CustomerRegisterResponse
    {
        public int ResendWaitTime { get; set; }
        public DateTime? LastSentDate { get; set; }
        public bool IsSent { get; set; }
    }
}