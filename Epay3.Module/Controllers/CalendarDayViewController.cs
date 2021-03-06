﻿using System;
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
using Epay3.Module.BusinessObjects.EpayDataModel;

namespace Epay3.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CalendarDayViewController : ViewController
    {
        public CalendarDayViewController()
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

        private void paGenerate_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var eParameterCurrentValue = e.ParameterCurrentValue.ToString();
            int year = int.Parse(eParameterCurrentValue);

            var firstDay = new DateTime(year, 1, 1);
            for (int i = 0; i < 365; i++)
            {
                var calendarDay = ObjectSpace.CreateObject<CalendarDay>();
                var date = firstDay.AddDays(i);
                calendarDay.Date = date;
                calendarDay.Day = date.DayOfYear;
                calendarDay.DayOfWeek = date.DayOfWeek;
                calendarDay.Year = date.Year;
                calendarDay.Weekend = (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
            }

            ObjectSpace.CommitChanges();
        }
    }
}