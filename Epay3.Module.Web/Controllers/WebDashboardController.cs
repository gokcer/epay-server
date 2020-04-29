using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;

namespace Epay3.Module.Web.Controllers
{
    public class WebDashboardController: ObjectViewController<DetailView, IDashboardData>
    {
        private WebDashboardViewerViewItem dashboardViewerViewItem;

        protected override void OnActivated()
        {
            base.OnActivated();
            dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
            if (dashboardViewerViewItem != null)
            {
                if (dashboardViewerViewItem.DashboardControl != null)
                {
                    dashboardViewerViewItem.DashboardControl.AllowExportDashboardItems = true;
                }
                else
                {
                    dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
                }
            }
        }

        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
        {
            ((WebDashboardViewerViewItem) sender).DashboardControl.AllowExportDashboardItems = true;
        }

        protected override void OnDeactivated()
        {
            if (dashboardViewerViewItem != null)
            {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
                dashboardViewerViewItem = null;
            }
            base.OnDeactivated();
        }
    }
}
