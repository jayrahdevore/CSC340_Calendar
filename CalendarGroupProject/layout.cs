using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGroupProject
{
    class CalendarEvent
    {
        
    }
    class Day
    {

        public List<CalendarEvent> events;
        public DateTime day;

        public Day(DateTime day)
        {
            // maybe construct this with a SQL statement?
            // maybe filter by day?
            this.day = day;
            
        }
    }

    class Month
    {

        public List<Day> days_of_month;

        public Month(int monthnum)
        {
            // Construct month

            // get days of month
            // construct using day
            DateTime month_start = new DateTime(2022, monthnum, 1);

            days_of_month = new List<Day>();

            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            for (DateTime date = month_start; date.Month == monthnum; date = myCal.AddDays(date, 1))
            {
                days_of_month.Add(new Day(date));
            }
        }
    }
}
