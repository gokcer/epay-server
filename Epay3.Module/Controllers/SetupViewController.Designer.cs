namespace Epay3.Module.Controllers
{
    partial class SetupViewController
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
            this.simpleActionImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionImport
            // 
            this.simpleActionImport.Caption = "Import";
            this.simpleActionImport.ConfirmationMessage = null;
            this.simpleActionImport.Id = "3040403f-6570-4393-97ba-1fdc18df647f";
            this.simpleActionImport.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.Site);
            this.simpleActionImport.ToolTip = null;
            this.simpleActionImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImport_Execute);
            // 
            // SetupViewController
            // 
            this.Actions.Add(this.simpleActionImport);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImport;
    }
}
