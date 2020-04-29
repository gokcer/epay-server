namespace Epay3.Module.Controllers
{
    partial class CardViewController
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
            this.paCharge = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.paInvalidate = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // paCharge
            // 
            this.paCharge.AcceptButtonCaption = null;
            this.paCharge.CancelButtonCaption = null;
            this.paCharge.Caption = "Charge";
            this.paCharge.Category = "ObjectsCreation";
            this.paCharge.ConfirmationMessage = null;
            this.paCharge.Id = "4fc4c8b2-6da5-401b-9fca-25f0959edce4";
            this.paCharge.ToolTip = null;
            this.paCharge.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.paCharge_CustomizePopupWindowParams);
            this.paCharge.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.paCharge_Execute);
            // 
            // paInvalidate
            // 
            this.paInvalidate.AcceptButtonCaption = null;
            this.paInvalidate.CancelButtonCaption = null;
            this.paInvalidate.Caption = "Invalidate Card";
            this.paInvalidate.ConfirmationMessage = null;
            this.paInvalidate.Id = "d4d4ce65-5c82-4060-912f-0e8665a13c5c";
            this.paInvalidate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.paInvalidate.TargetObjectsCriteria = "Active = true";
            this.paInvalidate.ToolTip = null;
            this.paInvalidate.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.paInvalidate_CustomizePopupWindowParams);
            this.paInvalidate.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.paInvalidate_Execute);
            // 
            // CardViewController
            // 
            this.Actions.Add(this.paCharge);
            this.Actions.Add(this.paInvalidate);
            this.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.Card);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction paCharge;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction paInvalidate;
    }
}
