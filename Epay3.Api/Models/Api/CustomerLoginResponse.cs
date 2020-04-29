using Epay3.Api.Controllers;

namespace Epay3.Api.Models.Api
{
    public class CustomerLoginResponse
    {
        public Configuration Configuration { get; set; }
        public  CustomerReportResponse CustomerReport { get; set; }
    }
}