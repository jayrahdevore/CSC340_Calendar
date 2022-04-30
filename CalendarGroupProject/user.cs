using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGroupProject
{
    internal class user
    {
        
        int user_id;
        string user_name;
        string user_type;

        public user (int user_id)
        {
            this.user_id = user_id;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT user_name, user_type FROM users WHERE user_id=@uid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    // user_id exists
                    user_name = myReader["user_name"].ToString();
                    user_type = myReader["user_type"].ToString();
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }

        public override string ToString()
        {
            return user_name;
        }

        public int getUserID()
        {
            return user_id;
        }

        public static bool IsValidUser(int user_id)
        {
            bool is_in_db = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT user_id FROM users WHERE user_id=@uid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    // user_id exists
                    is_in_db = myReader.HasRows;
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            return is_in_db;
        }


        public bool isOccupied(DateTime start, DateTime end)
        {
            return Event.isOccupied(user_id, start, end);
        }

        public void addEvent(string event_name, string event_description, DateTime start_time, DateTime end_time)
        {
            // attempt to add event to database

            //bool result = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // get index value
                string sql = "SELECT MAX(event_id) + 1 AS next_idx FROM events;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                MySqlDataReader myReader = cmd.ExecuteReader();
                int event_id = -1;
                while (myReader.Read())
                {
                    event_id = int.Parse(myReader["next_idx"].ToString());
                }
                Console.WriteLine("EID: " + event_id);
                myReader.Close();
                // WARNING: suceptible to SQL injection... maybe make description binary? (at least sanitize)
                // add event to event table
                sql = "INSERT INTO events (event_id, event_name, event_description, start_time, end_time)"
                    + "VALUES (@eid, @event_name, @event_description, @start_time, @end_time);";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eid", event_id);
                cmd.Parameters.AddWithValue("@event_name", event_name);
                cmd.Parameters.AddWithValue("@event_description", event_description);
                cmd.Parameters.AddWithValue("@start_time", start_time.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@end_time", end_time.ToString(StaticData.dateFormat));
                myReader = cmd.ExecuteReader();
                myReader.Close();
                /// finally add to map table
                sql = "INSERT INTO event_map (user_id, event_id)"
                +"VALUES (@uid, @eid);";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", user_id);
                cmd.Parameters.AddWithValue("@eid", event_id);
                myReader = cmd.ExecuteReader();
                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            //return true;
        }

        public bool isManager()
        {
            return user_type.Equals("manager");
        }

        public List<Event> eventsOnDate(DateTime day)
        {
            return Event.userEventsWithinDate(user_id, day.Add(-day.TimeOfDay), day.AddDays(1).Add(-day.TimeOfDay));
        }
        public List<Event> eventsInMonth(DateTime day)
        {
            DateTime start = day.Add(-day.TimeOfDay).AddDays(-day.Day); // beginning of month
            return Event.userEventsWithinDate(user_id, start, start.AddMonths(1));
        }

        static public List<user> listAllUsers()
        {
            // return list of users in database
            List<int> user_ids = new List<int>();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT user_id FROM users";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    user_ids.Add(int.Parse(myReader["user_id"].ToString()));
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            List<user> users = new List<user>();

            foreach (int id in user_ids)
            {
                users.Add(new user(id));
            }
            return users;
        }
    }
}
