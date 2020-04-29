using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using Epay3.Module.BusinessObjects.EpayDataModel;
using Epay3.Module.Services;

namespace Epay3.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomerViewController : ViewController
    {
        public CustomerViewController()
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

        private void paSendMessage_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var customerMessage = Application.CreateObjectSpace(typeof(NPCustomerMessage));
            var npNewDailyCharge = customerMessage.CreateObject<NPCustomerMessage>();

            var detailView = Application.CreateDetailView(customerMessage, npNewDailyCharge);
            detailView.ViewEditMode = ViewEditMode.Edit;
            e.View = detailView;
        }

        private void paSendMessage_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var customerMessage = (NPCustomerMessage) e.PopupWindowViewCurrentObject;
            OneSignalService oneSignalService = new OneSignalService();
            foreach (Customer customer in e.SelectedObjects)
            {
                var notificationToken = customer.NotificationToken;
                var title = customerMessage.Header;
                var content = customerMessage.Message;
                if (!string.IsNullOrWhiteSpace(notificationToken))
                {
                    oneSignalService.SendToClient(notificationToken, title,
                        content);
                    var message = ObjectSpace.CreateObject<Message>();
                    message.Date = DateTime.Now;
                    message.To = customer;
                    message.Title = title;
                    message.Content = content;
                }
            }

            ObjectSpace.CommitChanges();
        }

    }
}