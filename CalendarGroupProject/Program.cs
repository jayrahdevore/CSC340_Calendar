using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarGroupProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            testDays();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            

        }

        static void testDays()
        {
            // this method is designed to construct a list of days to populate the calendar

            // testing month class... remove later
            Month thisMonth = new Month(4);
            System.Diagnostics.Debug.Write("Testing debug console...\n");
            int monthnum = 4;

            // initialize list of days
            List<Day> calendar_days = new List<Day>();

            // start list of days on Sunday
            DateTime tmpday = new DateTime(2022, monthnum, 1);
            for (; tmpday.DayOfWeek != DayOfWeek.Sunday; tmpday = tmpday.AddDays(-1)) { }

            // populate with 35 consecutive days
            for (; calendar_days.Count < 42; tmpday = tmpday.AddDays(1))
            {
                calendar_days.Add(new Day(tmpday));
            }

            foreach (Day d in calendar_days)
            {
                System.Diagnostics.Debug.Write(d.day.ToString() + "\n");
            }

        }
    }
}
