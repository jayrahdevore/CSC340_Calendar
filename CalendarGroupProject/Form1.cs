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
        private int current_month;
        Event selected_event;

        public Form1()
        {
            InitializeComponent();


            //make sure all panels are initialized to not visible EXCEPT login screen
            btn_login.Visible = true;
            calendarPanel.Visible = false;
            addEventPanel.Visible = false;
            viewMonthlyEventPanel.Visible = false;
            //eventOptionsPanel.Visible = false;
            //viewEventPanel.Visible = false;
            //editEventPanel.Visible = false;
            //coordinateMeetingPanel.Visible = false;

            selected_event = null;
            init_calendar();
            

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
            updateEditView();
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
            foreach (Event ev in current_user.eventsInMonth(new DateTime(2022, current_month, 1))) {
                monthlyEventListListBox.Items.Add(ev);
            }
            
            calendarPanel.Visible = false;
        }

        //when clicking an event within the monthly event view panel
        private void monthlyEventListListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void updateEditView()
        {
            // populate fields if editing existing event
            if (selected_event != null)
            {
                tb_event_title.Text = selected_event.ToString();
                tb_event_details.Text = selected_event.getDescription();
                dt_enter_start.Value = selected_event.getStartTime();
                dt_enter_end.Value = selected_event.getEndTime();
                lb_event_members.Items.Clear();
                foreach (user u in selected_event.usersInEvent())
                {
                    lb_event_members.Items.Add(u);
                }
            }
            
            lb_all_members.Items.Clear();
            foreach (user u in user.listAllUsers())
            {
                lb_all_members.Items.Add(u);
            }
        }



        //confirm event added on add event panel
        private void buttonAddEvent_Click(object sender, EventArgs e)
        {
            string box_msg = "Event added.";
            string box_title = "Calendar App";
            

            if (current_user.isOccupied(dt_enter_start.Value, dt_enter_end.Value))
            {
                System.Windows.Forms.MessageBox.Show("Time already occupied");

            } else
            {
                if (selected_event == null)
                {
                    
                    constructSelectedEvent();
                    MessageBox.Show(box_msg, box_title);
                } else
                {
                    if (selected_event.isOccupied(dt_enter_start.Value, dt_enter_end.Value))
                    {
                        System.Windows.Forms.MessageBox.Show("Time overlap exists");
                    } else
                    {
                        selected_event.setEventName(tb_event_title.Text);
                        selected_event.setEventDescription(tb_event_details.Text);
                        selected_event.setStartTime(dt_enter_start.Value);
                        selected_event.setEndTime(dt_enter_end.Value);
                        selected_event.updateDB();
                        System.Windows.Forms.MessageBox.Show("Updated Event");
                    }
                }
                
                addEventPanel.Visible = false;
                calendarPanel.Visible = true;
                
                current_month = monthSelect.SelectedIndex + 1;
                fillCalendar();
            }
            

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

        void fillCalendar()
        {

            // this method is designed to construct a list of days to populate the calendar

            int monthnum = current_month;

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
                calendar_listboxes[i].Items.Clear();

                foreach (Event e in current_user.eventsOnDate(d.day)) {

                    calendar_listboxes[i].Items.Add(e);
                    calendar_listboxes[i].Enabled = false;
                }
                
                
            }


        }


        private void monthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            current_month = monthSelect.SelectedIndex + 1;
            fillCalendar();
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
                        lb_event_members.Enabled = true;
                        lb_all_members.Enabled = true;
                        btn_add_member.Enabled = true;
                        btn_delete_member.Enabled = true;
                    } else
                    {
                        lb_event_members.Enabled = false;
                        lb_all_members.Enabled = false;
                        btn_add_member.Enabled = false;
                        btn_delete_member.Enabled = false;
                    }

                    loginPanel.Visible = false;
                    
                    monthSelect.SelectedIndex = DateTime.Now.Month - 1;
                    calendarPanel.Visible = true;
                } else
                {
                    System.Windows.Forms.MessageBox.Show("User ID not in DB");
                }
                
            } else
            {
                System.Windows.Forms.MessageBox.Show("Invalid user ID");
            }
                

        }

        private void btn_delete_event_Click(object sender, EventArgs e)
        {
            selected_event.deleteEvent();
            selected_event = null;
            fillCalendar();
            calendarPanel.Visible = true;
            addEventPanel.Visible = false;
            
        }

        private void btn_add_member_Click(object sender, EventArgs e)
        {
            // populate if event exists, else create the event so we can work on it
            if (selected_event == null)
            {
                constructSelectedEvent();
            }
            selected_event.addMember((user)lb_all_members.SelectedItem);

            updateEditView();
        }

        private void constructSelectedEvent()
        {
            // create event
            selected_event = new Event(tb_event_title.Text.ToString(), tb_event_details.Text.ToString(),
                dt_enter_start.Value, // start time
                dt_enter_end.Value); // end time
        }

        private void btn_delete_member_Click(object sender, EventArgs e)
        {
            selected_event.removeMember((user)lb_event_members.SelectedItem);
            updateEditView();
        }

        private void btn_edit_event_Click(object sender, EventArgs e)
        {
            // button for editing an event
            selected_event = (Event)monthlyEventListListBox.SelectedItem;
            if (selected_event == null)
                return;
            viewMonthlyEventPanel.Visible = false;
            addEventPanel.Visible = true;
            updateEditView();
        }

    }
}
