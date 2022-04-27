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
        private static String connStr = "server=172.25.0.3;user=root;database=calendar;port=3306;password=example;";
        int user_id;

        public user (int user_id)
        {
            this.user_id = user_id;
        }

        public static bool IsValidUser(int user_id)
        {
            bool is_in_db = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
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
                    is_in_db = true;
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

        public bool isManager()
        {
            bool result = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT user_type FROM users WHERE user_id=@uid;";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", user_id);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    if (myReader["user_type"].ToString().Equals("manager"))
                    {
                        // user is a manager
                        result = true;
                    }
                    
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
    }
}
