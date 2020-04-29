using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class CalendarDayCalendarDaysCalendarCalendars
    {
        public int? Calendars { get; set; }
        public int? CalendarDays { get; set; }
        public int Oid { get; set; }
        public int? OptimisticLockField { get; set; }

        public virtual CalendarDay CalendarDaysNavigation { get; set; }
        public virtual Calendar CalendarsNavigation { get; set; }
    }
}
