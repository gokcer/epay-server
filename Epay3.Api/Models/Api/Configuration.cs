using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Epay3.Api.Models.Api
{
    public class Configuration
    {
        public decimal MinimumBalanceLimit { get; set; }
        public int DeviceMode { get; set; }
        public int ProductType { get; set; }

        public bool? CanCharge { get; set; }
        public bool? CanSale { get; set; }
        public bool? CanManageTable { get; set; }
        public IList<TransactionItemResponse> TransactionItems { get; set; }
        public IList<TransacionResponse> Transactions { get; set; }
        public IList<ConfigurationClient> Clients { get; set; }
        public IList<ConfigurationOrderStatus> AllowedOrderStatuses { get; set; }
        public string NotificationToken { get; set; }
        public int? PollPeriod { get; set; }
    }

    public class ConfigurationClient {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ConfigurationOrderStatus
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public ConfigurationOrderStatus() { }

        public ConfigurationOrderStatus(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
