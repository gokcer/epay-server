using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Api
{
    public class UpdateOrderStatusRequest
    {
        public int? IdOrder { get; set; }
        public int? IdOrderDetail { get; set; }
        public int Status { get; set; }
    }
}
