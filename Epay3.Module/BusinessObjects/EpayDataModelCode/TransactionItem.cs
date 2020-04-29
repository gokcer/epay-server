using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [DefaultClassOptions]
    public partial class TransactionItem
    {
        public TransactionItem(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Type = ETransactionType.Withdraw;
            ProductType = EProductType.Money;
        }

        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 40,DetailViewImageEditorFixedWidth = 300)]
        public Byte[] Image { get; set; }
    }

}
