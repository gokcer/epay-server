using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [DefaultClassOptions]
    public partial class Customer
    {
        public Customer(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
            NotificationToken = Guid.NewGuid().ToString();
        }
    }

}
