using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Gr8Food
{

    public class DBHelper
    {
        // |DataDirectory| automatically points to 
        // wherever the project folder is
        // this makes it work on ALL computers
        private static string connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;" +
             "AttachDbFilename=C:\\Users\\Austin\\source\\repos\\Gr8Food\\Gr8Food\\Gr8FoodDB.mdf;" +
             "Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
