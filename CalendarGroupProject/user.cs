using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGroupProject
{
    internal class User
    {

        SQLConnection my_conn;

        public int ID { get; set; }
        public string name { get; set; }

        public User(SQLConnection my_conn, int user_id)
        {
            this.my_conn = my_conn;
            this.ID = user_id;
        }

        public User(SQLConnection my_conn, int user_id, string name)
        {
            this.my_conn = my_conn;
            this.ID = user_id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public static List<User> GetAllUsers(SQLConnection my_conn)
        {
            // returns list of users from database
            string sql = "SELECT user_id, user_name FROM users";
            MySqlCommand cmd = my_conn.GetCommandFromString(sql);
            MySqlDataReader myReader = cmd.ExecuteReader();
            List<(int, string)> user_ids = new List<(int, string)>();
            while (myReader.Read())
            {
                user_ids.Add(
                    (
                        int.Parse(myReader["user_id"].ToString()),
                        myReader["user_name"].ToString()
                    )
                );
            }
            myReader.Close();
            List<User> users = new List<User>();
            user_ids.ForEach(data => users.Add(new User(my_conn, data.Item1, data.Item2)));
            return users;
        }

        public static bool IsManager(SQLConnection my_conn, int user_id)
        {
            // returns true of user is of type manager

            bool is_manager = false;

            string sql = "SELECT user_id FROM managers WHERE user_id=@user_id";
            MySqlCommand cmd = my_conn.GetCommandFromString(sql);
            cmd.Parameters.AddWithValue("user_id", user_id);
            MySqlDataReader myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                is_manager = true;
            }

            myReader.Close();
            return is_manager;
        }

        public List<Event> EventsWithinDate(int user_id, DateTime start, DateTime end)
        {
            /// return list of events on a given day for the user with a given ID
            List<Event> events = new List<Event>();

            try
            {
                string sql = "SELECT * FROM events RIGHT JOIN event_map ON event_map.event_id = events.event_id "
                    + "WHERE ((start_time >= @start_time AND start_time < @end_time) OR (start_time <= @start_time AND end_time > @end_time)) AND user_id = @uid;";

                MySqlCommand cmd = my_conn.GetCommandFromString(sql);
                // subtract time of day to get midnight
                cmd.Parameters.AddWithValue("@start_time", start);
                cmd.Parameters.AddWithValue("@end_time", end);
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    events.Add(new Event(myReader));
                }
                myReader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return events;

        }

        public List<Event> EventsWithinDate(DateTime start, DateTime end)
        {
            /// return list of events on a given day for the user
            List<Event> events = new List<Event>();

            try
            {
                string sql = "SELECT * FROM events RIGHT JOIN event_map ON event_map.event_id = events.event_id "
                    + "WHERE ((start_time >= @start_time AND start_time < @end_time) OR (start_time <= @start_time AND end_time > @end_time)) AND user_id = @uid;";

                MySqlCommand cmd = my_conn.GetCommandFromString(sql);
                // subtract time of day to get midnight
                cmd.Parameters.AddWithValue("@start_time", start);
                cmd.Parameters.AddWithValue("@end_time", end);
                cmd.Parameters.AddWithValue("@uid", ID);
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    events.Add(new Event(myReader));
                    Console.WriteLine("Found event");
                }
                myReader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return events;

        }

    }

    internal class Manager : User
    {

        public Manager(SQLConnection my_conn, int user_id) : base(my_conn, user_id) { }


    }
}
