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
    public partial class TransactionItem : XPObject
    {
        string fName;
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        decimal fPrice;
        public decimal Price
        {
            get { return fPrice; }
            set { SetPropertyValue<decimal>(nameof(Price), ref fPrice, value); }
        }
        Epay3.Module.BusinessObjects.ETransactionType fType;
        public Epay3.Module.BusinessObjects.ETransactionType Type
        {
            get { return fType; }
            set { SetPropertyValue<Epay3.Module.BusinessObjects.ETransactionType>(nameof(Type), ref fType, value); }
        }
        bool fActive;
        public bool Active
        {
            get { return fActive; }
            set { SetPropertyValue<bool>(nameof(Active), ref fActive, value); }
        }
        bool fSystem;
        public bool System
        {
            get { return fSystem; }
            set { SetPropertyValue<bool>(nameof(System), ref fSystem, value); }
        }
        Epay3.Module.BusinessObjects.EProductType fProductType;
        public Epay3.Module.BusinessObjects.EProductType ProductType
        {
            get { return fProductType; }
            set { SetPropertyValue<Epay3.Module.BusinessObjects.EProductType>(nameof(ProductType), ref fProductType, value); }
        }
        string fSpecialCode;
        public string SpecialCode
        {
            get { return fSpecialCode; }
            set { SetPropertyValue<string>(nameof(SpecialCode), ref fSpecialCode, value); }
        }
        TransactionItemCategory fCategory;
        [Association(@"TransactionItemReferencesTransactionItemCategory")]
        public TransactionItemCategory Category
        {
            get { return fCategory; }
            set { SetPropertyValue<TransactionItemCategory>(nameof(Category), ref fCategory, value); }
        }
        [Association(@"TransactionDetailReferencesProduct")]
        public XPCollection<TransactionDetail> TransactionDetails { get { return GetCollection<TransactionDetail>(nameof(TransactionDetails)); } }
        [Association(@"OrderDetailReferencesTransactionItem")]
        public XPCollection<OrderDetail> OrderDetails { get { return GetCollection<OrderDetail>(nameof(OrderDetails)); } }
        [Association(@"ItemPermissionReferencesTransactionItem")]
        public XPCollection<ItemPermission> ItemPermissions { get { return GetCollection<ItemPermission>(nameof(ItemPermissions)); } }
    }

}
