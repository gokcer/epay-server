namespace Epay3.Api.Models.Api
{
    public class CardResponse
    {
        public string CardNo { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public int? MinimumBalanceLimit { get; set; }
    }
}