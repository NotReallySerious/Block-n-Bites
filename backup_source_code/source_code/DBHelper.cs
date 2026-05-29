using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace Block_n_bites
{
    public class DBHelper
    {
        // change this to match YOUR database name
        private static string connectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=Gr8FoodDB;Integrated Security=True";

        // returns a connection object that forms can use
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
