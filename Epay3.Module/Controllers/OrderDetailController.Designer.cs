namespace Epay3.Module.Controllers
{
    partial class OrderDetailController
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
            this.saCancel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // saCancel
            // 
            this.saCancel.Caption = "Cancel";
            this.saCancel.Category = "RecordEdit";
            this.saCancel.ConfirmationMessage = null;
            this.saCancel.Id = "623481bc-2be4-4481-9a7b-795ea86b76f0";
            this.saCancel.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.OrderDetail);
            this.saCancel.ToolTip = null;
            this.saCancel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saCancel_Execute);
            // 
            // OrderDetailController
            // 
            this.Actions.Add(this.saCancel);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction saCancel;
    }
}
