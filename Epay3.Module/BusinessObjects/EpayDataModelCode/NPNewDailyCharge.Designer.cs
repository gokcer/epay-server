﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [NonPersistent]
    public partial class NPNewDailyCharge : XPObject
    {
        TransactionItem fItem;
        public TransactionItem Item
        {
            get { return fItem; }
            set { SetPropertyValue<TransactionItem>(nameof(Item), ref fItem, value); }
        }
        decimal fQuantity;
        public decimal Quantity
        {
            get { return fQuantity; }
            set { SetPropertyValue<decimal>(nameof(Quantity), ref fQuantity, value); }
        }
        Calendar fCalendar;
        public Calendar Calendar
        {
            get { return fCalendar; }
            set { SetPropertyValue<Calendar>(nameof(Calendar), ref fCalendar, value); }
        }
    }

}
