namespace Epay3.Module.Controllers
{
    partial class CalendarDayViewController
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
            this.paGenerate = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // paGenerate
            // 
            this.paGenerate.Caption = "Generate Year";
            this.paGenerate.Category = "ObjectsCreation";
            this.paGenerate.ConfirmationMessage = null;
            this.paGenerate.Id = "83de2223-c6c5-4fde-bf2e-6e36bbbbb86c";
            this.paGenerate.NullValuePrompt = null;
            this.paGenerate.ShortCaption = null;
            this.paGenerate.ToolTip = null;
            this.paGenerate.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.paGenerate_Execute);
            // 
            // CalendarDayViewController
            // 
            this.Actions.Add(this.paGenerate);
            this.TargetObjectType = typeof(Epay3.Module.BusinessObjects.EpayDataModel.CalendarDay);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.ParametrizedAction paGenerate;
    }
}
