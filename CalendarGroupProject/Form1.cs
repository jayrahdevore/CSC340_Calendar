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
        public Form1()
        {
            InitializeComponent();

            //make sure all panels are initialized to not visible EXCEPT home screen
            calendarPanel.Visible = true;
            addEventPanel.Visible = false;
            viewMonthlyEventPanel.Visible = false;
            eventOptionsPanel.Visible = false;
            viewEventPanel.Visible = false;
            editEventPanel.Visible = false;
            coordinateMeetingPanel.Visible = false;
        }

        private void mondayLabel_Click(object sender, EventArgs e)
        {
            //ACCIDENTALLY DOUBLE CLICKED A RANDOM LABEL, IGNORE THIS!
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
    }
}
