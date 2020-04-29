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
    public partial class TransactionDetail : XPObject
    {
        Transaction fTransaction;
        [Association(@"TransactionDetailReferencesTransaction")]
        public Transaction Transaction
        {
            get { return fTransaction; }
            set { SetPropertyValue<Transaction>(nameof(Transaction), ref fTransaction, value); }
        }
        TransactionItem fItem;
        [Association(@"TransactionDetailReferencesProduct")]
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
        decimal fTotal;
        public decimal Total
        {
            get { return fTotal; }
            set { SetPropertyValue<decimal>(nameof(Total), ref fTotal, value); }
        }
        DateTime? fValidFrom;
        public DateTime? ValidFrom
        {
            get { return fValidFrom; }
            set { SetPropertyValue<DateTime?>(nameof(ValidFrom), ref fValidFrom, value); }
        }
        DateTime? fValidTo;
        public DateTime? ValidTo
        {
            get { return fValidTo; }
            set { SetPropertyValue<DateTime?>(nameof(ValidTo), ref fValidTo, value); }
        }
        bool fIsCancel;
        public bool IsCancel
        {
            get { return fIsCancel; }
            set { SetPropertyValue<bool>(nameof(IsCancel), ref fIsCancel, value); }
        }
    }

}
