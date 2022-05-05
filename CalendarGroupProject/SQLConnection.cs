using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGroupProject
{
    internal class SQLConnection
    {
        MySqlConnection conn;
        private string sql_conn_str = "server=172.18.0.2;user=root;database=calendar;port=3306;password=example;";

        public SQLConnection()
        {
            // instantiate and open connection
            conn = new MySqlConnection(sql_conn_str);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
       
        public void Close()
        {
            // close and void connection
            if (conn == null)
            {
                Console.WriteLine("Warning: Connection was null");
                return;
            }
            conn.Close();
        }

        public MySqlCommand GetCommandFromString(string sql)
        {
            // returns a mysql command handle from current instance
            return new MySqlCommand(sql, conn);
        }


    }
}
