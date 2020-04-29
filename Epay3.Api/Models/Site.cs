using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Site
    {
        public int Oid { get; set; }
        public string Name { get; set; }
        public int? Slot { get; set; }
        public string RegistrationCode { get; set; }
        public int? OrderCooldownTime { get; set; }
        public int? MinimumBalanceLimit { get; set; }
        public bool? FeatureCustomerMenu { get; set; }
        public bool? FeatureLoginWithCard { get; set; }
        public bool? FeatureCustomerPortal { get; set; }
        public bool? FeatureKiosk { get; set; }
        public bool? FeatureChargeOnline { get; set; }
        public bool? FeatureCardBalanceTransfer { get; set; }
        public bool? FeatureShopList { get; set; }
        public decimal? TransactionFee { get; set; }
        public decimal? TransactionMonthlyLimit { get; set; }
        public int? Timezone { get; set; }
        public int? OptimisticLockField { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? RadiusMeter { get; set; }
        public int? CustomerProvisionTime { get; set; }
        public int? WaitressProvisionTime { get; set; }
        public bool? NotifyCustomerForTransaction { get; set; }
        public bool? EnableRemoteOrders { get; set; }
        public bool? EnableLocationOrders { get; set; }
    }
}
