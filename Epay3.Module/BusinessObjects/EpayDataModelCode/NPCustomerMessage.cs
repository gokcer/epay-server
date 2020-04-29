using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    public partial class NPCustomerMessage
    {
        public NPCustomerMessage(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
