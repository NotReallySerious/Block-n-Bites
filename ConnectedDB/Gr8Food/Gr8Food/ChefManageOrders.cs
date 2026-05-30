using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gr8Food
{
    public partial class ChefManageOrders : Form
    {
        Chef loggedinchef;
        public ChefManageOrders(Chef newChef)
        {
            loggedinchef = newChef;
            InitializeComponent();
        }

        private void ChefManageOrders_Load(object sender, EventArgs e)
        {
            foreach (string item in loggedinchef.LoadOrders())
            {
                lstOrders.Items.Add(item);
            }
            foreach (string item in loggedinchef.LoadOrderIDs())
            {
                cboSelectedOrder.Items.Add(item);
            }
        }

        

        private void cboSelectedOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectedOrder.SelectedItem == null) return;

            

            string orderID = cboSelectedOrder.SelectedItem.ToString();

            

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                
                string query = @"SELECT o.OrderID, u.Name AS CustomerName, 
                         m.Name AS ItemName,
                         o.TotalCost, o.Status
                         FROM Orders o
                         JOIN Users u ON o.CustomerID = u.UserID
                         JOIN MenuItems m ON o.ItemID = m.ItemID
                         WHERE o.Status NOT IN ('Cancelled', 'Completed') AND o.OrderID = @OrderID";

                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // show order summary on label
                    lblOrderDetails.Text = "OrderID: "+reader["OrderID"].ToString() + "\nCustomer Name: " +
                                     reader["CustomerName"].ToString() + "\nItem Name: " +
                                     reader["ItemName"].ToString() + "\nPrice: RM" +
                                     reader["TotalCost"].ToString() + "\nStatus: " +
                                     reader["Status"].ToString();
                    
                }
                
            }
            
    }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            string display = loggedinchef.UpdateOrderStatus(cboSelectedOrder.Text);
            MessageBox.Show(display);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstOrders.Items.Clear();
            cboSelectedOrder.Items.Clear();
            foreach (string item in loggedinchef.LoadOrders())
            {
                lstOrders.Items.Add(item);
            }
            foreach (string item in loggedinchef.LoadOrderIDs())
            {
                cboSelectedOrder.Items.Add(item);
            }
        }
    }
}
