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
        // if event_id is -1, add it to database and update
        public int event_id { get; private set; }
        public string event_name { get; set; }
        public string event_description { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }

        public override string ToString()
        {
            return event_name;
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

        public Event(string event_name, string event_description,
            DateTime start_time, DateTime end_time)
        {
            // 
            event_id = -1;
            this.event_name = event_name;
            this.event_description = event_description;
            this.start_time = start_time;
            this.end_time = end_time;
        }

        public void SyncToDB(SQLConnection conn)
        {
            // Sync changes to DB
            try
            {
                string sql;
                MySqlCommand cmd;
                MySqlDataReader myReader;
                // get index value to set if not known
                if (event_id == -1)
                {
                    // get index value
                    sql = "SELECT MAX(event_id) + 1 AS next_idx FROM events;";
                    cmd = conn.GetCommandFromString(sql);
                    myReader = cmd.ExecuteReader();
                    int tmp_id = 0; // sets to 0 if null
                    if (myReader.Read())
                    {
                        int.TryParse(myReader["next_idx"].ToString(), out tmp_id);
                    }
                    event_id = tmp_id;
                    myReader.Close();
                    // WARNING: suceptible to SQL injection... maybe make description binary? (at least sanitize)
                    // add event to event table
                    sql = "INSERT INTO events (event_id, event_name, event_description, start_time, end_time)"
                        + "VALUES (@eid, @event_name, @event_description, @start_time, @end_time);";
                    cmd = conn.GetCommandFromString(sql);
                    cmd.Parameters.AddWithValue("@eid", event_id);
                    cmd.Parameters.AddWithValue("@event_name", event_name);
                    cmd.Parameters.AddWithValue("@event_description", event_description);
                    cmd.Parameters.AddWithValue("@start_time", start_time);
                    cmd.Parameters.AddWithValue("@end_time", end_time);
                    myReader = cmd.ExecuteReader();
                    myReader.Close();
                }
                else
                {
                    // Else update values
                    sql = "UPDATE events SET "
                        + "event_name=@event_name, event_description=@event_description, "
                        + "start_time=@start_time, end_time=@end_time WHERE event_id=@eid;";
                    cmd = conn.GetCommandFromString(sql);
                    cmd.Parameters.AddWithValue("@eid", event_id);
                    cmd.Parameters.AddWithValue("@event_name", event_name);
                    cmd.Parameters.AddWithValue("@event_description", event_description);
                    cmd.Parameters.AddWithValue("@start_time", start_time);
                    cmd.Parameters.AddWithValue("@end_time", end_time);
                    myReader = cmd.ExecuteReader();
                    myReader.Close();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public List<User> GetParticipants(SQLConnection conn)
        {
            // return list of users scheduled for event
            List <User> participants = new List<User>();

            // returns list of user IDs associated with event
            string sql = "SELECT user_id FROM event_map WHERE event_id=@event_id";
            MySqlCommand cmd = conn.GetCommandFromString(sql);
            cmd.Parameters.AddWithValue("@event_id", event_id);
            MySqlDataReader myReader = cmd.ExecuteReader();

            // make list of users
            List<int> user_ids = new List<int>();
            while (myReader.Read())
            {
                user_ids.Add((int.Parse(myReader["user_id"].ToString()))
                );
            }
            myReader.Close();

            // now that the reader is closed, we can create the list of users
            user_ids.ForEach(u => participants.Add(new User(conn, u)));

            return participants;
        }

        public void AddParticipant(SQLConnection conn, User toadd)
        {
            // adds participant to DB; syncs if event hasn't been created yet
            if (event_id == -1)
            {
                SyncToDB(conn);
            }
            // skip if participant is already included
            if (GetParticipants(conn).Any(participant => participant.ID == toadd.ID))
            {
                return; // already added
            }
            // else add to DB -- Note!  This is a syncing change
            try
            {
                string sql = "INSERT INTO event_map (user_id, event_id) VALUES (@uid, @evid);";
                MySqlCommand cmd = conn.GetCommandFromString(sql);
                cmd.Parameters.AddWithValue("@evid", event_id);
                cmd.Parameters.AddWithValue("@uid", toadd.ID);
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public void RemoveParticipant(SQLConnection conn, User toadd)
        {
            // syncing change to DB: removes participant
            try
            {
                string sql = "DELETE FROM event_map WHERE user_id=@uid AND event_id=@evid;";
                MySqlCommand cmd = conn.GetCommandFromString(sql);
                cmd.Parameters.AddWithValue("@evid", event_id);
                cmd.Parameters.AddWithValue("@uid", toadd.ID);
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void Delete(SQLConnection conn)
        {
            // delete from DB

            // first remove all users
            GetParticipants(conn).ForEach(p => RemoveParticipant(conn, p));

            // then remove events
            try
            {
                string sql = "DELETE FROM events WHERE event_id=@evid;";
                MySqlCommand cmd = conn.GetCommandFromString(sql);
                cmd.Parameters.AddWithValue("@evid", event_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
