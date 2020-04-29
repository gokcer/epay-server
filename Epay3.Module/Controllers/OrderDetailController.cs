using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Epay3.Module.BusinessObjects;
using Epay3.Module.BusinessObjects.EpayDataModel;

namespace Epay3.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class OrderDetailController : ViewController
    {
        public OrderDetailController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void saCancel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var timezone = ObjectSpace.GetObjects<Site>().First().Timezone;
            var siteNow = DateTime.UtcNow.AddHours(timezone);

            foreach (OrderDetail orderDetail in e.SelectedObjects)
            {
                if (orderDetail.Status == EOrderStatus.Canceled) continue; // skip cancelled orders

                // set order status
                orderDetail.Status = EOrderStatus.Canceled;

                // find related transaction. If exists create counter record.
                var orderDetailTransactionDetail = orderDetail.TransactionDetail;
                if (orderDetailTransactionDetail != null)
                {
                    // create counter transaction
                    var transaction = ObjectSpace.CreateObject<Transaction>();
                    var transactionDetail = ObjectSpace.CreateObject<TransactionDetail>();
                    var orderTransaction = orderDetailTransactionDetail.Transaction;
                    transaction.Card = orderTransaction.Card;
                    transaction.ProductType = orderTransaction.ProductType;
                    transaction.DateEffective = orderTransaction.Date;
                    transactionDetail.Item = orderDetailTransactionDetail.Item;
                    transactionDetail.Quantity = orderDetailTransactionDetail.Quantity * -1; // inverse quantity
                    transactionDetail.Total = orderDetailTransactionDetail.Total * -1; // inverse total
                    transaction.TransactionDetails.Add(transactionDetail);
                    transaction.UpdateTotalsAndDate(siteNow);
                    transaction.Date = orderTransaction.Date; // todo burası parametrik yapılacak.
                    transaction.IsCancel = true;
                    transactionDetail.IsCancel = true;
                    orderDetail.CancelTransactionDetail = transactionDetail;
                }
            }
            ObjectSpace.CommitChanges();
        }
    }
}
