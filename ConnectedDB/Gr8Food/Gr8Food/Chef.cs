using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gr8Food
{
    public class Chef
    {
        public string ChefID;
        public string ChefName;
        string Role = "chef";
        public string ChefEmail;
        public Chef(string ID, string Name, string Email)
        {
            ChefID = ID;
            ChefName = Name;
            ChefEmail = Email;
        }
        public Chef()
        {

        }


        public List<string> LoadMenu()
        {
            List<string> menuList = new List<string>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();



                string query = "SELECT * FROM MenuItems";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                //Keep Looping when record is still found
                while (reader.Read())
                {
                    // format each row 
                    string display = reader["Name"].ToString() + " | " +
                                     reader["Category"].ToString() + " | RM" +
                                     reader["Price"].ToString() + " | " +
                                     reader["Status"].ToString();
                    //Add the output into a list
                    menuList.Add(display);



                }
                // return the list
                return menuList;
            }
        }
        public List<string> LoadMenuNames()
        {
            List<string> nameList = new List<string>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "SELECT Name FROM MenuItems";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nameList.Add(reader["Name"].ToString());
                }
            }

            return nameList;
        }
        public bool EditMenuItem(string name, string category,
                            decimal price, string status, string itemID) 
        {
            // validate price
            decimal parsedPrice; 
            // Check if the inputed price is valid
           

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE MenuItems SET Name=@Name, Category=@Category, " +
                               "Price=@Price, Status=@Status WHERE ItemID=@ItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ItemID", itemID);

                cmd.ExecuteNonQuery();
            }

            return true;
        }
        public void AddMenuItem(string name, string category,
                           decimal price)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO MenuItems (Name, Category, Price, Status, ChefID) " +
                               "VALUES (@Name, @Category, @Price, @Status, @ChefID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Status", "Available"); // default to available
                cmd.Parameters.AddWithValue("@ChefID", ChefID); // To ensure chef gets the recognition

                cmd.ExecuteNonQuery();
            }
        }
        public void RemoveMenuItem(string ItemID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM MenuItems WHERE ItemID = @ItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.ExecuteNonQuery();
            }
        }
        public List<string> LoadOrders()
        {
            List<string> orderList = new List<string>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                // Show User Name instead of USer ID + Show Item name instead of Item ID
                string query = @"SELECT o.OrderID, u.Name AS CustomerName, 
                         m.Name AS ItemName,
                         o.TotalCost, o.Status
                         FROM Orders o
                         JOIN Users u ON o.CustomerID = u.UserID
                         JOIN MenuItems m ON o.ItemID = m.ItemID
                         WHERE o.Status NOT IN ('Cancelled', 'Completed')";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    string display = reader["OrderID"].ToString() + " | " +
                                     reader["CustomerName"].ToString() + " | " +
                                     reader["ItemName"].ToString() + " | RM" +
                                     reader["TotalCost"].ToString() + " | " +
                                     reader["Status"].ToString();

                    orderList.Add(display);
                }
            }

            return orderList;
        }

  
        
        public List<string> LoadOrderIDs()
        {
            List<string> idList = new List<string>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT o.OrderID FROM Orders o
                             WHERE o.Status NOT IN ('Cancelled', 'Completed')";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idList.Add(reader["OrderID"].ToString());
                }
            }

            return idList;
        }
        public string UpdateOrderStatus(string orderID)
        {
            string currentStatus = "";
            string newStatus = "";


            // get current status
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                currentStatus = cmd.ExecuteScalar().ToString();
            }

            //check status so  it can only move forward
            if (currentStatus.Trim() == "Pending")
            {
                newStatus = "In Progress";

            }

            else if (currentStatus.Trim() == "In Progress")
            {
                newStatus = "Completed";
            }
            else
            {
                return "Unable to Process";
            }

            //update the status
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                // Update the order + give the chef who completed it some credit
                string query = "UPDATE Orders SET Status = @Status, ChefID = @ChefID " +
                               "WHERE OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@ChefID", ChefID);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.ExecuteNonQuery();
            }

            return $"Status sucessfully updated to: {newStatus}"; // tell form it worked
        }
    }
}

