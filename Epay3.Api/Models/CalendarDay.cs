using System;
using System.Collections.Generic;

namespace Epay3.Api.Models
{
    public partial class CalendarDay
    {
        public CalendarDay()
        {
            CalendarDayCalendarDaysCalendarCalendars = new HashSet<CalendarDayCalendarDaysCalendarCalendars>();
        }

        public int Oid { get; set; }
        public DateTime? Date { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Day { get; set; }
        public int? Year { get; set; }
        public bool? Weekend { get; set; }
        public int? OptimisticLockField { get; set; }

        public virtual ICollection<CalendarDayCalendarDaysCalendarCalendars> CalendarDayCalendarDaysCalendarCalendars { get; set; }
    }
}
