using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class Calendar
    {
        public Calendar()
        {
            CalendarDayCalendarDaysCalendarCalendars = new HashSet<CalendarDayCalendarDaysCalendarCalendars>();
        }

        public int Oid { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? OptimisticLockField { get; set; }

        public virtual ICollection<CalendarDayCalendarDaysCalendarCalendars> CalendarDayCalendarDaysCalendarCalendars { get; set; }
    }
}
