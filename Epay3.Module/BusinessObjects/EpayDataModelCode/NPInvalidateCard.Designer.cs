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
    public partial class NPInvalidateCard : XPObject
    {
        decimal fCurrentBalance;
        [DevExpress.ExpressApp.Model.ModelDefault("AllowEdit", "False")]
        public decimal CurrentBalance
        {
            get { return fCurrentBalance; }
            set { SetPropertyValue<decimal>(nameof(CurrentBalance), ref fCurrentBalance, value); }
        }
        decimal fDepositAmount;
        public decimal DepositAmount
        {
            get { return fDepositAmount; }
            set { SetPropertyValue<decimal>(nameof(DepositAmount), ref fDepositAmount, value); }
        }
        decimal fRefundAmount;
        public decimal RefundAmount
        {
            get { return fRefundAmount; }
            set { SetPropertyValue<decimal>(nameof(RefundAmount), ref fRefundAmount, value); }
        }
    }

}