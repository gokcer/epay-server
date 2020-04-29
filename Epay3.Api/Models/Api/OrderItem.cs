namespace Epay3.Api.Models.Api
{
    public class OrderItem
    {
        public int IdTransactionItem { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}