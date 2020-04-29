using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [DefaultClassOptions]
    [NavigationItem("Billing")]
    public partial class Bill
    {
        public Bill(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
