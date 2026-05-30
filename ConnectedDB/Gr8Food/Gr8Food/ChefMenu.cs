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
using System.Xml.Linq;

namespace Gr8Food
{
    public partial class ChefMenu : Form
    {
        Chef loggedinchef;
        public ChefMenu(Chef newchef)
        {
            loggedinchef = newchef;
            InitializeComponent();
        }

        private void ChefMenu_Load(object sender, EventArgs e)
        {
            // Add all the avaliable choices to the combo boxes
            cboCategory.Items.Add("Breakfast");
            cboCategory.Items.Add("Lunch");
            cboCategory.Items.Add("Dinner");
            cboCategory.Items.Add("Snacks");
            cboCategory.Items.Add("Drinks");
            cboStatus.Items.Add("Available");
            cboStatus.Items.Add("Unavailable");
            //Load the orders to list box
            foreach (string item in loggedinchef.LoadMenu())
            {
                lstMenu.Items.Add(item);
            }
            // Load the names only for selecting
            foreach (string item in loggedinchef.LoadMenuNames())
            {
                cboSelectedItem.Items.Add(item);
            }

        }

        private void cboSelectedItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                // if nothing selected yet, do nothing
                if (cboSelectedItem.SelectedItem == null) return;

                string selectedName = cboSelectedItem.SelectedItem.ToString();

                using (SqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // find that specific item in the database by name
                    string query = "SELECT * FROM MenuItems WHERE Name = @Name";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", selectedName);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // if the item is found, fill the fields
                    if (reader.Read())
                    {
                        txtItemName.Text = reader["Name"].ToString();
                        txtPrice.Text = reader["Price"].ToString();
                        cboCategory.Text = reader["Category"].ToString();
                        cboStatus.Text = reader["Status"].ToString();
                        lblItemID.Text = reader["ItemID"].ToString();
                        
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblRefresh_Click(object sender, EventArgs e)
        {
            lstMenu.Items.Clear();
            cboSelectedItem.Items.Clear();
            cboSelectedItem.SelectedIndex = -1;
            foreach (string item in loggedinchef.LoadMenu())
            {
                lstMenu.Items.Add(item);
            }
            // Load the names only for selecting
            foreach (string item in loggedinchef.LoadMenuNames())
            {
                cboSelectedItem.Items.Add(item);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // validate selection
            if (cboSelectedItem.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to edit.");
                return;
            }

            // validate fields are not empty
            if (txtItemName.Text == "" || txtPrice.Text == "" ||
                cboCategory.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            //  validate price
            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("Price must be a valid number greater than 0.");
                return;
            }
            loggedinchef.EditMenuItem(txtItemName.Text, cboCategory.Text, price, cboStatus.Text, lblItemID.Text);
            MessageBox.Show("Item Sucessfully Edited");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            loggedinchef.RemoveMenuItem(lblItemID.Text);
            MessageBox.Show("Item Sucessfully Deleted");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            ChefNewItem NewChefNewItem = new ChefNewItem(loggedinchef);
            NewChefNewItem.Show();
        }
    }
}
