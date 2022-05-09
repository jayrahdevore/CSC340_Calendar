using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarGroupProject
{
    public partial class Form1 : Form
    {
        // lists of labels and listboxes for month view
        static List<Label> calendar_labels;
        static List<ListBox> calendar_listboxes;

        // current selections
        private User current_user;
        private Event selected_event;

        // datetime for calendar and monthly view
        private DateTime selected_view_datetime;

        // persistent connection
        SQLConnection conn;

        public Form1()
        {

            // open SQL connection (class constants handle constant URL for now)
            conn = new SQLConnection();

            // set up forms
            InitializeComponent();
            InitCalendar();

            /* Initialize login Sequence */

            // list all users on login panel
            List<User> users = User.GetAllUsers(conn);

            // add user names to listbox
            users.ForEach(v => lb_users_login.Items.Add(v));

            loginPanel.BringToFront();

            // initialize as current day
            selected_view_datetime = DateTime.Now;


        }

        //Exit button to close the application from any panel
        private void btn_exit_Click(object sender, EventArgs e)
        {
            conn.Close();
            Application.Exit();
        }


        void InitCalendar()
        {
            calendar_labels = new List<Label>();
            calendar_listboxes = new List<ListBox>();

            calendar_labels.Add(l_cal00);
            calendar_labels.Add(l_cal01);
            calendar_labels.Add(l_cal02);
            calendar_labels.Add(l_cal03);
            calendar_labels.Add(l_cal04);
            calendar_labels.Add(l_cal05);
            calendar_labels.Add(l_cal06);
            calendar_labels.Add(l_cal10);
            calendar_labels.Add(l_cal11);
            calendar_labels.Add(l_cal12);
            calendar_labels.Add(l_cal13);
            calendar_labels.Add(l_cal14);
            calendar_labels.Add(l_cal15);
            calendar_labels.Add(l_cal16);
            calendar_labels.Add(l_cal20);
            calendar_labels.Add(l_cal21);
            calendar_labels.Add(l_cal22);
            calendar_labels.Add(l_cal23);
            calendar_labels.Add(l_cal24);
            calendar_labels.Add(l_cal25);
            calendar_labels.Add(l_cal26);
            calendar_labels.Add(l_cal30);
            calendar_labels.Add(l_cal31);
            calendar_labels.Add(l_cal32);
            calendar_labels.Add(l_cal33);
            calendar_labels.Add(l_cal34);
            calendar_labels.Add(l_cal35);
            calendar_labels.Add(l_cal36);
            calendar_labels.Add(l_cal40);
            calendar_labels.Add(l_cal41);
            calendar_labels.Add(l_cal42);
            calendar_labels.Add(l_cal43);
            calendar_labels.Add(l_cal44);
            calendar_labels.Add(l_cal45);
            calendar_labels.Add(l_cal46);

            calendar_listboxes.Add(lb_cal00);
            calendar_listboxes.Add(lb_cal01);
            calendar_listboxes.Add(lb_cal02);
            calendar_listboxes.Add(lb_cal03);
            calendar_listboxes.Add(lb_cal04);
            calendar_listboxes.Add(lb_cal05);
            calendar_listboxes.Add(lb_cal06);
            calendar_listboxes.Add(lb_cal10);
            calendar_listboxes.Add(lb_cal11);
            calendar_listboxes.Add(lb_cal12);
            calendar_listboxes.Add(lb_cal13);
            calendar_listboxes.Add(lb_cal14);
            calendar_listboxes.Add(lb_cal15);
            calendar_listboxes.Add(lb_cal16);
            calendar_listboxes.Add(lb_cal20);
            calendar_listboxes.Add(lb_cal21);
            calendar_listboxes.Add(lb_cal22);
            calendar_listboxes.Add(lb_cal23);
            calendar_listboxes.Add(lb_cal24);
            calendar_listboxes.Add(lb_cal25);
            calendar_listboxes.Add(lb_cal26);
            calendar_listboxes.Add(lb_cal30);
            calendar_listboxes.Add(lb_cal31);
            calendar_listboxes.Add(lb_cal32);
            calendar_listboxes.Add(lb_cal33);
            calendar_listboxes.Add(lb_cal34);
            calendar_listboxes.Add(lb_cal35);
            calendar_listboxes.Add(lb_cal36);
            calendar_listboxes.Add(lb_cal40);
            calendar_listboxes.Add(lb_cal41);
            calendar_listboxes.Add(lb_cal42);
            calendar_listboxes.Add(lb_cal43);
            calendar_listboxes.Add(lb_cal44);
            calendar_listboxes.Add(lb_cal45);
            calendar_listboxes.Add(lb_cal46);

        }

        void FillCalendar()
        {
            // populate the calendar view with events

            // start list of days on Sunday
            DateTime tmpday = new DateTime(selected_view_datetime.Year, selected_view_datetime.Month, 1);
            for (; tmpday.DayOfWeek != DayOfWeek.Monday; tmpday = tmpday.AddDays(-1)) { }

            // populate with 35 consecutive days
            for (int i = 0; i < 35; i++)
            {
                calendar_labels[i].Text = tmpday.Day.ToString();
                if (tmpday.Month != selected_view_datetime.Month)
                {
                    calendar_listboxes[i].BackColor = System.Drawing.Color.DimGray;
                    calendar_labels[i].BackColor = System.Drawing.Color.DimGray;
                }
                else
                {
                    calendar_listboxes[i].BackColor = System.Drawing.Color.DarkTurquoise;
                    calendar_labels[i].BackColor = System.Drawing.Color.DarkTurquoise;
                }
                calendar_listboxes[i].Items.Clear();

                foreach (Event e in current_user.EventsWithinDate(tmpday, tmpday.AddDays(1)))
                {

                    calendar_listboxes[i].Items.Add(e);
                    calendar_listboxes[i].Enabled = false;
                }
                tmpday = tmpday.AddDays(1);
            }

            // Updated Selected Index
            monthSelect.SelectedIndex = selected_view_datetime.Month - 1;


        }


        private void monthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update calendar to reflect selected month
            selected_view_datetime = new DateTime(
                (int) selected_view_datetime.Year,
                (int) monthSelect.SelectedIndex + 1,
                1
                );
            FillCalendar();

        }


        private void btn_login_Click(object sender, EventArgs e)
        {
            // complete login sequence

            int selected_user_id = ((User)lb_users_login.SelectedItem).ID;

            if (User.IsManager(conn, selected_user_id))
            {
                current_user = new Manager(conn, selected_user_id);
            }
            else
            {
                current_user = new User(conn, selected_user_id);
                // go ahead and hide the "schedule meeting" button if not a manager
                btn_schedule_meeting.Visible = false;
            }

            // Update Month View
            monthSelect.SelectedIndex = selected_view_datetime.Month - 1;

            // initialize main view
            loginPanel.Visible = false;
            GoToMainCalendarPanel();

        }

        private void ViewEventListButton_Click(object sender, EventArgs e)
        {
            // view a list of the month's events
            GoToMonthEventListPanel();
        }

        private void MonthEventReturnButton_Click(object sender, EventArgs e)
        {
            // go back to the main view
            GoToMainCalendarPanel();
        }

        private void backButtonAddEvent_Click(object sender, EventArgs e)
        {
            GoToMainCalendarPanel();
        }

        private void buttonAddEvent_Click(object sender, EventArgs e)
        {
            // add an event to the DB under the current user

            // first check for valid start and end time
            if (dt_add_event_time_start.Value > dt_add_event_time_end.Value)
            {
                System.Windows.Forms.MessageBox.Show("Start time is later than end time");
                return;
            }

            // then check to see if event can be added
                if (current_user.EventsWithinDate(dt_add_event_time_start.Value,
                dt_add_event_time_end.Value).Count != 0)
            {
                // An event exists within this time
                System.Windows.Forms.MessageBox.Show("Time already occupied");
                return;
            }

            // create an event from the data filled in
            // WARNING: SQL injection very likely possible
            Event to_add = new Event(
                tb_add_event_title.Text,
                tb_add_event_details.Text,
                dt_add_event_time_start.Value,
                dt_add_event_time_end.Value
                );

            // adding a participant syncs the event to the DB as well
            to_add.AddParticipant(conn, current_user);

            // Go back to main panel
            GoToMainCalendarPanel();
        }

        private void addEventButton_Click(object sender, EventArgs e)
        {
            addEventPanel.BringToFront();
        }

        private void DeleteEventButton_Click(object sender, EventArgs e)
        {
            // delete selecteed
            ((Event)MonthEventListBox.SelectedItem).Delete(conn);
            GoToMonthEventListPanel();

        }

        private void EditEventButton_Click(object sender, EventArgs e)
        {
            // edit event properties
            EditEventPanel.BringToFront();
            selected_event = (Event)MonthEventListBox.SelectedItem;

            tb_edit_event.Text = selected_event.event_name;
            tb_edit_event_details.Text = selected_event.event_description;
            dt_edit_event_start.Value = selected_event.start_time;
            dt_edit_event_end.Value = selected_event.end_time;
        }

        private void ButtonApplyEditEvent_Click(object sender, EventArgs e)
        {
            // apply changes to event
            selected_event.event_name = tb_edit_event.Text;
            selected_event.event_description = tb_edit_event_details.Text;
            selected_event.start_time = dt_edit_event_start.Value;
            selected_event.end_time = dt_edit_event_end.Value;

            // sync changes to DB
            selected_event.SyncToDB(conn);
            GoToMonthEventListPanel();
        }

        private void ButtonEditBack_Click(object sender, EventArgs e)
        {
            // go back to monthly event list
            GoToMonthEventListPanel();

        }

        private void GoToMonthEventListPanel()
        {
            // populate list
            DateTime month_start = selected_view_datetime.Add(
                -(selected_view_datetime.TimeOfDay)
                ).AddDays(selected_view_datetime.Day);

            // fill out month list
            MonthEventListBox.Items.Clear();
            current_user.EventsWithinDate(month_start, month_start.AddMonths(1)).ForEach(
                ev => MonthEventListBox.Items.Add(ev)
            );
            viewMonthlyEventPanel.BringToFront();
        }

        private void GoToMainCalendarPanel()
        {
            // Update Month View
            FillCalendar();
            // go to the main month calendar view
            calendarPanel.BringToFront();
        }

        private void btn_schedule_meeting_Click(object sender, EventArgs e)
        {
            PanelScheduleMeeting.BringToFront();

            // set up timespan options
            lb_schedule_meeting_timespan.Items.Clear();
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(0, 15, 0)); // 15 minutes
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(0, 30, 0)); // 30 minutes
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(0, 45, 0)); // 45 minutes
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(1, 0, 0)); // 1 hour
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(2, 0, 0)); // 2 hours
            lb_schedule_meeting_timespan.Items.Add(new TimeSpan(3, 0, 0)); // 3 hours

            cb_schedule_meeting_members.Items.Clear();
            User.GetAllUsers(conn).ForEach(
                user => cb_schedule_meeting_members.Items.Add(user)
                );

            
        }

        private void FillPotentialScheduledMeetingTimes()
        {
            // skip if timespan is not selected
            if (lb_schedule_meeting_timespan.SelectedIndex == -1)
            {
                return;
            }

            DateTime start_time = dt_schedule_meeting_start.Value;
            DateTime end_time = dt_schedule_meeting_end.Value;
            TimeSpan timespan = (TimeSpan)lb_schedule_meeting_timespan.SelectedItem;

            // normalize start time?
            //start_time

            // would like to simplify this into a lambda function
            List<User> selected_users = new List<User>();
            for (int i = 0; i < cb_schedule_meeting_members.CheckedIndices.Count; i++)
            {
                selected_users.Add((User) cb_schedule_meeting_members.CheckedItems[i]);
            }
            

           

            lb_schedule_meeting_availability.Items.Clear();
            while (start_time < end_time)
            {
                // add block if all selected users are available
                if (selected_users.All(u => u.EventsWithinDate(start_time,
                    start_time + timespan).Count == 0))
                {
                    lb_schedule_meeting_availability.Items.Add(start_time);
                }

                // increment next block
                start_time += (TimeSpan) lb_schedule_meeting_timespan.SelectedItem;
            }
            

        }


        private void btn_schedule_meeting_back_Click(object sender, EventArgs e)
        {
            GoToMainCalendarPanel();
        }

        private void cb_schedule_meeting_members_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPotentialScheduledMeetingTimes();
        }

        private void lb_schedule_meeting_timespan_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPotentialScheduledMeetingTimes();
        }

        private void dt_schedule_meeting_start_ValueChanged(object sender, EventArgs e)
        {
            FillPotentialScheduledMeetingTimes();
        }

        private void dt_schedule_meeting_end_ValueChanged(object sender, EventArgs e)
        {
            FillPotentialScheduledMeetingTimes();
        }

        private void cb_schedule_meeting_members_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            FillPotentialScheduledMeetingTimes();
        }

        private void btn_schedule_meeting_confirm_Click(object sender, EventArgs e)
        {
            // first check for valid start and end time
            
            if (lb_schedule_meeting_availability.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid time");
                return;
            }

            TimeSpan timespan = (TimeSpan)lb_schedule_meeting_timespan.SelectedItem;

            // create an event from the data filled in
            // WARNING: SQL injection very likely possible
            Event to_add = new Event(
                tb_schedule_meeting_title.Text,
                tb_schedule_meeting_details.Text,
                (DateTime)lb_schedule_meeting_availability.SelectedItem,
                (DateTime)lb_schedule_meeting_availability.SelectedItem + timespan
                );

            // need to sync first so we establish an id for later
            to_add.SyncToDB(conn);

            // adding a participant syncs the event to the DB as well
            // would like to simplify this into a lambda function
            for (int i = 0; i < cb_schedule_meeting_members.CheckedItems.Count; i++)
            {
                to_add.AddParticipant(conn, (User)cb_schedule_meeting_members.CheckedItems[i]);
            }

            // go back to main menu
            GoToMainCalendarPanel();
        }
    }
}
