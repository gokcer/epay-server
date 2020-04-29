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
using DevExpress.Utils.Extensions;
using Epay3.Module.BusinessObjects.EpayDataModel;

namespace Epay3.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TransactionViewController : ViewController
    {
        public TransactionViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (SecuritySystem.CurrentUserName != "gokcer") saCancelTransaction.Active["gokcer"] = false;

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

        private void saCancelTransaction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            HashSet<Order> ordersToDelete = new HashSet<Order>();
            foreach (Transaction transaction in e.SelectedObjects)
            {
                var transactionTransactionDetails = transaction.TransactionDetails.ToList();
                foreach (var transactionDetail in transactionTransactionDetails)
                {
                    // delete all orderdetails related with this transactiondetail
                    var orderDetails = ObjectSpace.GetObjectsQuery<OrderDetail>().Where(od=>od.TransactionDetail==transactionDetail).ToList();
                    orderDetails.ForEach(od=>
                    {
                        od.Delete();
                    });
                    transactionDetail.Delete();
                }

                ObjectSpace.GetObjectsQuery<Order>().Where(o => o.Transaction == transaction).ToList().ForEach(o=>o.Delete());

                transaction.Delete();
            }
            ObjectSpace.CommitChanges();

            // delete orphaned orders
            var orders = ObjectSpace.GetObjectsQuery<Order>().Where(o=>o.OrderDetails.Count==0).ToList();
            orders.ForEach(o=>o.Delete());
            ObjectSpace.CommitChanges();
        }

        private void deleteTransasctionDetail(TransactionDetail transactionDetail)
        {
            throw new NotImplementedException();
        }
    }
}
