using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGroupProject
{
   
    

    internal class Event
    {
        private int event_id;
        private string event_name;
        private string event_description;
        private DateTime start_time;
        private DateTime end_time;


        public override string ToString()
        {
            return event_name;
        }

        public string getDescription()
        {
            return event_description;
        }

        public DateTime getStartTime()
        {
            return start_time;
        }

        public DateTime getEndTime()
        {
            return end_time;
        }

        public void setStartTime(DateTime st)
        {
            start_time = st;
        }
        public void setEndTime(DateTime st)
        {
            end_time = st;
        }
        public void setEventName(String event_name)
        {
            this.event_name = event_name;
        }
        public void setEventDescription(String event_description)
        {
            this.event_description = event_description;
        }


        public Event(int event_id)
        {
            // event constructor from event ID
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT event_name, event_description, start_time, end_time FROM events WHERE event_id=@uid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", event_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    this.event_id = event_id;
                    event_name = myReader["event_name"].ToString();
                    event_description = myReader["event_description"].ToString();
                    start_time = DateTime.Parse(myReader["start_time"].ToString());
                    end_time = DateTime.Parse(myReader["end_time"].ToString());
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

        public Event(MySqlDataReader myReader)
        {
            // event constructor from SQL data reader entry
            
            event_id = int.Parse(myReader["event_id"].ToString());
            event_name = myReader["event_name"].ToString();
            event_description = myReader["event_description"].ToString();
            start_time = DateTime.Parse(myReader["start_time"].ToString());
            end_time = DateTime.Parse(myReader["end_time"].ToString());
        }

        public Event(string event_name, string event_description, DateTime start_time, DateTime end_time)
        {
            // constructor that first adds the items to the database
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // get index value
                string sql = "SELECT MAX(event_id) + 1 AS next_idx FROM events;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    if (!int.TryParse(myReader["next_idx"].ToString(), out event_id))
                    {
                        event_id = 0;
                    }
                    
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

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            this.event_description = event_description;
            this.start_time = start_time;
            this.end_time = end_time;
        }

        public List<user> usersInEvent()
        {
            // return list of users in event
            List<int> user_ids = new List<int>();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT user_id FROM event_map WHERE event_id=@uid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", event_id);
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

        public static bool isOccupied(int user_id, DateTime start, DateTime end)
        {
            bool isOccupied = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                string sql = "SELECT events.event_id FROM events RIGHT JOIN event_map ON event_map.event_id = events.event_id "
                    + "WHERE start_time > @start_time AND end_time < @end_time AND user_id = @uid;";
                conn.Open();
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                // subtract time of day to get midnight
                cmd.Parameters.AddWithValue("@start_time", start.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@end_time", end.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    isOccupied = true;
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return isOccupied;

        }

        public bool isOccupied(DateTime start, DateTime end)
        {
            // tests if time slot is occupied
            bool isOccupied = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                string sql = "SELECT events.event_id FROM events RIGHT JOIN event_map ON event_map.event_id = events.event_id "
                    + "WHERE start_time > @start_time AND end_time < @end_time AND events.event_id = @uid;";
                conn.Open();
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                // subtract time of day to get midnight
                cmd.Parameters.AddWithValue("@start_time", start.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@end_time", end.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@uid", event_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    isOccupied = true;
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return isOccupied;

        }

        public void deleteEvent()
        {
            // remove data from the event map and event tables in DB
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            
            try
            {
                string sql = "DELETE FROM event_map WHERE event_id=@eid;";
                conn.Open();
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eid", event_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();

                sql = "DELETE FROM events WHERE event_id=@eid;";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@eid", event_id);
                myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
          


        public static List<Event> userEventsWithinDate(int user_id, DateTime start, DateTime end)
        {
            /// return list of events on a given day for the user
            List<Event> events = new List<Event>();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                string sql = "SELECT * FROM events RIGHT JOIN event_map ON event_map.event_id = events.event_id "
                    + "WHERE (start_time >= @start_day) AND ((end_time > @next_day) OR (end_time > @start_day) AND user_id = @uid;";
                conn.Open();
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                // subtract time of day to get midnight
                cmd.Parameters.AddWithValue("@start_day", start.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@next_day", end.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    events.Add(new Event(myReader));
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return events;

        }

        public void addMember(user toadd)
        {
            // add a member to the event mapping in DB

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO event_map (user_id, event_id) VALUES (@uid, @evid);";
                Console.WriteLine("DEBUG: user_id: " + toadd.getUserID().ToString() + " " + event_id);
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@evid", event_id);
                cmd.Parameters.AddWithValue("@uid", toadd.getUserID());
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }

        public void removeMember(user toremove)
        {
            // add a member to the event mapping in DB

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "DELETE FROM event_map WHERE user_id=@uid AND event_id=@evid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@evid", event_id);
                cmd.Parameters.AddWithValue("@uid", toremove.getUserID());
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }

        public void updateDB()
        {
            // update DB with changes

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(StaticData.sqlConnStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "UPDATE events SET event_name=@event_name, event_description=@event_description,"
                    + " start_time=@start_time, end_time=@end_time WHERE event_id=@eid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eid", event_id);
                cmd.Parameters.AddWithValue("@event_name", event_name);
                cmd.Parameters.AddWithValue("@event_description", event_description);
                cmd.Parameters.AddWithValue("@start_time", start_time.ToString(StaticData.dateFormat));
                cmd.Parameters.AddWithValue("@end_time", end_time.ToString(StaticData.dateFormat));
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

        }


    }
}

