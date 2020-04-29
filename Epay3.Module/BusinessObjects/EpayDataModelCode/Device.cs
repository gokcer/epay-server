using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [DefaultClassOptions]
    [NavigationItem("Settings")]
    [DefaultProperty("Serial")]
    public partial class Device
    {
        public Device(Session session) : base(session)
        {
            
        }

        public override void AfterConstruction()
        {
            this.fDeviceToken = Guid.NewGuid().ToString();
            base.AfterConstruction();
        }
    }

}
