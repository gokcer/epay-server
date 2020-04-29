using System.Collections.Generic;
using Epay3.Api.Controllers;

namespace Epay3.Api.Models.Api
{
    public class CompleteOrderRequest
    {
        public List<OrderItem> OrderItems { get; set; }
    }
}