namespace Epay3.Api.Models.Api
{
    public class CreateCardRequest
    {
        public string CitizenshipNumber { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Phone { get; set; }

        public bool Returnable { get; set; }
    }
}