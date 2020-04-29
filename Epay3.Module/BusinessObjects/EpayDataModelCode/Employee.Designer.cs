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

    [DeferredDeletion(false)]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class Employee : Customer
    {
        bool fActive;
        public bool Active
        {
            get { return fActive; }
            set { SetPropertyValue<bool>(nameof(Active), ref fActive, value); }
        }
        bool fCanCharge;
        public bool CanCharge
        {
            get { return fCanCharge; }
            set { SetPropertyValue<bool>(nameof(CanCharge), ref fCanCharge, value); }
        }
        bool fCanSale;
        public bool CanSale
        {
            get { return fCanSale; }
            set { SetPropertyValue<bool>(nameof(CanSale), ref fCanSale, value); }
        }
        bool fCanManageTable;
        public bool CanManageTable
        {
            get { return fCanManageTable; }
            set { SetPropertyValue<bool>(nameof(CanManageTable), ref fCanManageTable, value); }
        }
        bool fCanResetOrder;
        public bool CanResetOrder
        {
            get { return fCanResetOrder; }
            set { SetPropertyValue<bool>(nameof(CanResetOrder), ref fCanResetOrder, value); }
        }
        bool fCanCancelOrder;
        public bool CanCancelOrder
        {
            get { return fCanCancelOrder; }
            set { SetPropertyValue<bool>(nameof(CanCancelOrder), ref fCanCancelOrder, value); }
        }
        bool fCanCompleteOrder;
        public bool CanCompleteOrder
        {
            get { return fCanCompleteOrder; }
            set { SetPropertyValue<bool>(nameof(CanCompleteOrder), ref fCanCompleteOrder, value); }
        }
        bool fCanStartOrder;
        public bool CanStartOrder
        {
            get { return fCanStartOrder; }
            set { SetPropertyValue<bool>(nameof(CanStartOrder), ref fCanStartOrder, value); }
        }
        [Association(@"TransactionReferencesEmployee")]
        public XPCollection<Transaction> Transactions { get { return GetCollection<Transaction>(nameof(Transactions)); } }
        [Association(@"DeviceLoginReferencesEmployee")]
        public XPCollection<DeviceLogin> DeviceLogins { get { return GetCollection<DeviceLogin>(nameof(DeviceLogins)); } }
        [Association(@"LocationCardReferencesEmployee")]
        public XPCollection<LocationCard> LocationCards { get { return GetCollection<LocationCard>(nameof(LocationCards)); } }
    }

}
