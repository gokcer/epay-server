namespace Epay3.Api.Models.Api
{
    public class LoginResponse
    {
        public string AuthToken { get; set; }
        public int IdEmployee { get; set; }
        public Configuration Configuration { get; set; }
    }
}