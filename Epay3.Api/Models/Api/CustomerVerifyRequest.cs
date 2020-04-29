namespace Epay3.Api.Models.Api
{
    public class CustomerVerifyRequest
    {
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
    }
}