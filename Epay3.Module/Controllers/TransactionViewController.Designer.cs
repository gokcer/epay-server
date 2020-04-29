namespace Epay3.Module.Controllers
{
    partial class TransactionViewController
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
            this.saCancelTransaction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // saCancelTransaction
            // 
            this.saCancelTransaction.Caption = "Cancel Transaction";
            this.saCancelTransaction.Category = "Edit";
            this.saCancelTransaction.ConfirmationMessage = "Transaction will be canceled permamently. Are you sure?";
            this.saCancelTransaction.Id = "957490b1-0fd8-488c-90a4-1f9e4921a9e2";
            this.saCancelTransaction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.saCancelTransaction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.saCancelTransaction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.saCancelTransaction.ToolTip = null;
            this.saCancelTransaction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.saCancelTransaction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saCancelTransaction_Execute);
            // 
            // TransactionViewController
            // 
            this.Actions.Add(this.saCancelTransaction);
            this.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.Transaction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction saCancelTransaction;
    }
}
