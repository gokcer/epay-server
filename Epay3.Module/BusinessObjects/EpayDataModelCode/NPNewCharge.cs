﻿using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    public partial class NPNewCharge
    {
        public NPNewCharge(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
