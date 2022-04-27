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

        private user current_user;

        public Form1()
        {
            InitializeComponent();


            //make sure all panels are initialized to not visible EXCEPT home screen
            btn_login.Visible = true;
            calendarPanel.Visible = true;
            addEventPanel.Visible = false;
            viewMonthlyEventPanel.Visible = false;
            eventOptionsPanel.Visible = false;
            viewEventPanel.Visible = false;
            editEventPanel.Visible = false;
            coordinateMeetingPanel.Visible = false;

            init_calendar();

            monthSelect.SelectedIndex = DateTime.Now.Month - 1;

        }

        //Exit button to close the application from any panel
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //"Add Event" button takes user to add event panel
        private void addEventButton_Click(object sender, EventArgs e)
        {
            calendarPanel.Visible = false;
            addEventPanel.Visible = true;
        }

        //back button from "Add Event" menu to home screen
        private void backButtonAddEvent_Click(object sender, EventArgs e)
        {
            calendarPanel.Visible = true;
            addEventPanel.Visible = false;
        }

        //Title label, "Calendar App", user can click to return to home screen
        //make sure all panels aside from calendarPanel are set to false in this method
        private void titleLabel_Click(object sender, EventArgs e)
        {
            calendarPanel.Visible = true;
            addEventPanel.Visible = false;
            viewMonthlyEventPanel.Visible = false;
            eventOptionsPanel.Visible = false;
            viewEventPanel.Visible = false;
            editEventPanel.Visible = false;
            coordinateMeetingPanel.Visible = false;
        }

        //back button to go back to home from monthly event viewer panel
        private void monthlyEventListBackButton_Click(object sender, EventArgs e)
        {
            viewMonthlyEventPanel.Visible = false;
            calendarPanel.Visible = true;
        }

        //button to show monthly event list panel, initialize listbox contents here?
        private void viewEventListButton_Click(object sender, EventArgs e)
        {
            viewMonthlyEventPanel.Visible = true;
            monthlyEventListListBox.Items.Clear();
            monthlyEventListListBox.Items.Add("Event 1");
            monthlyEventListListBox.Items.Add("Event 2");
            calendarPanel.Visible = false;
        }

        //when clicking an event within the monthly event view panel
        private void monthlyEventListListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewMonthlyEventPanel.Visible = false;
            eventOptionsPanel.Visible = true;
        }

        //delete event confirmation button
        private void button3_Click(object sender, EventArgs e)
        {
            string box_msg = "Are you sure you want to delete?";
            string box_title = "Calendar App";
            MessageBox.Show(box_msg, box_title);
        }

        //button to view event details once an event has been selected
        private void button1_Click(object sender, EventArgs e)
        {
            viewEventPanel.Visible = true;
            eventOptionsPanel.Visible = false;
        }

        //button to go back from details view to the options screen
        private void button4_Click(object sender, EventArgs e)
        {
            viewEventPanel.Visible = false;
            eventOptionsPanel.Visible = true;
        }

        //back button from options screen to monthly event planner view
        private void button5_Click(object sender, EventArgs e)
        {
            eventOptionsPanel.Visible = false;
            viewMonthlyEventPanel.Visible = true;
        }

        //Edit Event Details button
        private void button2_Click(object sender, EventArgs e)
        {
            eventOptionsPanel.Visible = false;
            editEventPanel.Visible = true;
        }

        //back button to return to options panel from edit event panel
        private void button6_Click(object sender, EventArgs e)
        {
            editEventPanel.Visible = false;
            eventOptionsPanel.Visible = true;
        }

        //confirm changes save on edit panel
        private void button7_Click(object sender, EventArgs e)
        {
            string box_msg = "Changes saved.";
            string box_title = "Calendar App";
            MessageBox.Show(box_msg, box_title);
            editEventPanel.Visible = false;
            calendarPanel.Visible = true;
        }

        //confirm event added on add event panel
        private void buttonAddEvent_Click(object sender, EventArgs e)
        {
            string box_msg = "Event added.";
            string box_title = "Calendar App";
            MessageBox.Show(box_msg, box_title);
            addEventPanel.Visible = false;
            calendarPanel.Visible = true;
        }

        //coordinate meeting button opens the manager menu
        private void coordinateMeetingButton_Click(object sender, EventArgs e)
        {
            calendarPanel.Visible = false;
            coordinateMeetingPanel.Visible = true;
        }

        //back from corrdinate meeting to home page
        private void button8_Click(object sender, EventArgs e)
        {
            calendarPanel.Visible = true;
            coordinateMeetingPanel.Visible = false;
        }

        static List<Label> calendar_labels;
        static List<ListBox> calendar_listboxes;

        void init_calendar()
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

        void fillCalendar(int monthnum)
        {
            
            // this method is designed to construct a list of days to populate the calendar


            // testing month class... remove later
            System.Diagnostics.Debug.Write("Testing debug console...\n");
            //int monthnum = 4;

            // initialize list of days
            List<Day> calendar_days = new List<Day>();

            // start list of days on Sunday
            DateTime tmpday = new DateTime(2022, monthnum, 1);
            for (; tmpday.DayOfWeek != DayOfWeek.Monday; tmpday = tmpday.AddDays(-1)) { }

            // populate with 35 consecutive days
            for (; calendar_days.Count < 35; tmpday = tmpday.AddDays(1))
            {
                calendar_days.Add(new Day(tmpday));
            }

            for (int i = 0; i <calendar_days.Count(); i++)
            {
                Day d = calendar_days[i];
                System.Diagnostics.Debug.Write(d.day.ToString() + "\n");
                calendar_labels[i].Text = d.day.Day.ToString();
                if (d.day.Month != monthnum)
                {
                    calendar_listboxes[i].BackColor = System.Drawing.Color.DimGray;
                    calendar_labels[i].BackColor = System.Drawing.Color.DimGray;
                } else
                {
                    calendar_listboxes[i].BackColor = System.Drawing.Color.DarkTurquoise;
                    calendar_labels[i].BackColor = System.Drawing.Color.DarkTurquoise;
                }
            }


        }


        private void monthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month_idx = monthSelect.SelectedIndex + 1;
            fillCalendar(month_idx);
        }


        private void btn_login_Click(object sender, EventArgs e)
        {

            // initialize user
            int user_id;
            
            if (int.TryParse(tb_login.Text, out user_id)) {
                // check if ID is in DB
                if (user.IsValidUser(user_id))
                {
                    this.current_user = new user(user_id);

                    if (current_user.isManager()) {
                        btn_coordinate_meeting.Enabled = true;
                    } else
                    {
                        btn_coordinate_meeting.Visible = false;
                    }

                    loginPanel.Visible = false;
                } else
                {
                    System.Windows.Forms.MessageBox.Show("User ID not in DB");
                }
                
            } else
            {
                System.Windows.Forms.MessageBox.Show("Invalid user ID");
            }
                

        }
    }
}
