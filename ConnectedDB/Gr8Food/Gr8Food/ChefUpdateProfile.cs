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
    public partial class ChefUpdateProfile : Form
    {
        Chef loggedinchef;
        public ChefUpdateProfile(Chef newChef)
        {
            loggedinchef = newChef;
            InitializeComponent();
            

            
            txtName.Text = loggedinchef.ChefName;
            txtEmail.Text = loggedinchef.ChefEmail;
        
            // password is left empty on purpose for security
        }

        private void ChefUpdateProfile_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // validate name and email are not empty
            if (txtName.Text == "" || txtEmail.Text == "" || txtPassword.Text == "") 
            {
                MessageBox.Show("Name, Email or Password cannot be empty.");
                return;
            }

           // update the database
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Users SET Name=@Name, Email=@Email, Password=@Password " +
                               "WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@UserID", loggedinchef.ChefID);

                cmd.ExecuteNonQuery();
            }


            // so other forms that use loggedInChef still show correct info
            loggedinchef.ChefName = txtName.Text;
            loggedinchef.ChefEmail = txtEmail.Text;

            MessageBox.Show("Profile updated successfully!");
            this.Close();
        }
    }
}
