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
    public partial class Device : XPObject
    {
        string fSerial;
        public string Serial
        {
            get { return fSerial; }
            set { SetPropertyValue<string>(nameof(Serial), ref fSerial, value); }
        }
        [Persistent(@"DeviceToken")]
        [MemberDesignTimeVisibility(false)]
        string fDeviceToken;
        [PersistentAlias("fDeviceToken")]
        public string DeviceToken
        {
            get { return fDeviceToken; }
        }
        Epay3.Module.BusinessObjects.EDeviceMode fDeviceMode;
        public Epay3.Module.BusinessObjects.EDeviceMode DeviceMode
        {
            get { return fDeviceMode; }
            set { SetPropertyValue<Epay3.Module.BusinessObjects.EDeviceMode>(nameof(DeviceMode), ref fDeviceMode, value); }
        }
        Epay3.Module.BusinessObjects.EProductType fProductType;
        public Epay3.Module.BusinessObjects.EProductType ProductType
        {
            get { return fProductType; }
            set { SetPropertyValue<Epay3.Module.BusinessObjects.EProductType>(nameof(ProductType), ref fProductType, value); }
        }
        int fPollPeriod;
        public int PollPeriod
        {
            get { return fPollPeriod; }
            set { SetPropertyValue<int>(nameof(PollPeriod), ref fPollPeriod, value); }
        }
        bool fMaskOrderCustomer;
        public bool MaskOrderCustomer
        {
            get { return fMaskOrderCustomer; }
            set { SetPropertyValue<bool>(nameof(MaskOrderCustomer), ref fMaskOrderCustomer, value); }
        }
        bool fKioskMode;
        public bool KioskMode
        {
            get { return fKioskMode; }
            set { SetPropertyValue<bool>(nameof(KioskMode), ref fKioskMode, value); }
        }
        int fCompletedOrderKeepDuration;
        public int CompletedOrderKeepDuration
        {
            get { return fCompletedOrderKeepDuration; }
            set { SetPropertyValue<int>(nameof(CompletedOrderKeepDuration), ref fCompletedOrderKeepDuration, value); }
        }
        [Association(@"TransactionReferencesDevice")]
        public XPCollection<Transaction> Transactions { get { return GetCollection<Transaction>(nameof(Transactions)); } }
        [Association(@"DeviceLoginReferencesDevice")]
        public XPCollection<DeviceLogin> DeviceLogins { get { return GetCollection<DeviceLogin>(nameof(DeviceLogins)); } }
    }

}
