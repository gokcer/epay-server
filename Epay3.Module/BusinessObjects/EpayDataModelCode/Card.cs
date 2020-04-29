using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{
    [DefaultClassOptions]
    [DefaultProperty("CardNo")]
    public partial class Card
    {
        public Card(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
