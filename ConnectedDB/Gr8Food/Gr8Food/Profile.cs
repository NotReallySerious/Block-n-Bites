using Gr8Food;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Gr8Food
{
    public partial class Profile : Form
    {
        private readonly string _connectionString;

        public Profile()
        {
            InitializeComponent();
            DBHelper.GetConnection();
            this.Load += Profile_Load;
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                            "SELECT Name, Email, Role FROM Users WHERE Name = @name";

                        cmd.Parameters.AddWithValue("@name", Session.CurrentUserName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text =
                                    reader["Name"] != DBNull.Value
                                        ? reader["Name"].ToString()
                                        : string.Empty;

                                txtEmail.Text =
                                    reader["Email"] != DBNull.Value
                                        ? reader["Email"].ToString()
                                        : string.Empty;

                                lblRoleValue.Text =
                                    reader["Role"] != DBNull.Value
                                        ? reader["Role"].ToString()
                                        : "—";
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Could not find your user record.",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load profile: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtNewPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name cannot be empty.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(password))
            {
                if (password.Length < 6)
                {
                    MessageBox.Show("New password must be at least 6 characters.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPassword.Focus();
                    return;
                }
                if (password != confirm)
                {
                    MessageBox.Show("New password and Confirm password do not match.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Focus();
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        if (string.IsNullOrEmpty(password))
                        {
                            cmd.CommandText =
                                "UPDATE Users SET Name = @newName, Email = @email " +
                                "WHERE Name = @currentName";
                        }
                        else
                        {
                            cmd.CommandText =
                                "UPDATE Users SET Name = @newName, Email = @email, " +
                                "Password = @pass WHERE Name = @currentName";
                            cmd.Parameters.AddWithValue("@pass", password);
                        }

                        cmd.Parameters.AddWithValue("@newName", name);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@currentName", Session.CurrentUserName);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            Session.CurrentUserName = name;
                            txtNewPassword.Text = string.Empty;
                            txtConfirmPassword.Text = string.Empty;
                            MessageBox.Show("Profile updated successfully!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No changes were saved. Please try again.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    MessageBox.Show(
                        "That email address is already in use by another account.\n" +
                        "Please choose a different email.",
                        "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Failed to update profile: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update profile: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}