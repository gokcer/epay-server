namespace Epay3.Module.Controllers
{
    partial class CustomerViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.paSendMessage = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // paSendMessage
            // 
            this.paSendMessage.AcceptButtonCaption = null;
            this.paSendMessage.CancelButtonCaption = null;
            this.paSendMessage.Caption = "Send Message";
            this.paSendMessage.ConfirmationMessage = null;
            this.paSendMessage.Id = "57f25104-3515-4ea2-a3e8-a12f231283bc";
            this.paSendMessage.ToolTip = null;
            this.paSendMessage.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.paSendMessage_CustomizePopupWindowParams);
            this.paSendMessage.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.paSendMessage_Execute);
            // 
            // CustomerViewController
            // 
            this.Actions.Add(this.paSendMessage);
            this.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.Customer);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction saSendLink;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction paSendMessage;
    }
}
