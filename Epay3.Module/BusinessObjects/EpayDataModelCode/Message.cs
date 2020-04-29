using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{
    [DefaultClassOptions]
    public partial class Message
    {
        public Message(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
