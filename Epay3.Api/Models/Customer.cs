using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Card = new HashSet<Card>();
            DeviceLogin = new HashSet<DeviceLogin>();
            ItemPermission = new HashSet<ItemPermission>();
            Location = new HashSet<Location>();
            LocationCard = new HashSet<LocationCard>();
            Message = new HashSet<Message>();
            Order = new HashSet<Order>();
            RegistrationRequest = new HashSet<RegistrationRequest>();
            Station = new HashSet<Station>();
            Transaction = new HashSet<Transaction>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public string CitizenshipNumber { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Team { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? ObjectType { get; set; }
        public bool? Active { get; set; }
        public bool? CanCharge { get; set; }
        public string NotificationToken { get; set; }
        public bool? CanSale { get; set; }
        public bool? CanManageTable { get; set; }
        public bool? CanResetOrder { get; set; }
        public bool? CanCancelOrder { get; set; }
        public bool? CanCompleteOrder { get; set; }
        public bool? CanStartOrder { get; set; }

        public virtual XpobjectType ObjectTypeNavigation { get; set; }
        public virtual ICollection<Card> Card { get; set; }
        public virtual ICollection<DeviceLogin> DeviceLogin { get; set; }
        public virtual ICollection<ItemPermission> ItemPermission { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<LocationCard> LocationCard { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<RegistrationRequest> RegistrationRequest { get; set; }
        public virtual ICollection<Station> Station { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
