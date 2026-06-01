using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Gr8Food
{
    public partial class frmProfile : Form
    {
        // ── Connection string ─────────────────────────────────────────────────────────
        private readonly string _connectionString =
            System.Configuration.ConfigurationManager
                  .ConnectionStrings["Gr8FoodConnection"]
                  ?.ConnectionString
            ?? throw new InvalidOperationException(
                   "Connection string 'Gr8FoodConnection' not found.");

        // Constructor ─────────────────────────────────────────────────────────────────
        public frmProfile()
        {
            InitializeComponent();
            this.Load += Profile_Load;
        }

        // ── On load: populate fields from DB using Session.CurrentUserName ────────────
        private void Profile_Load(object? sender, EventArgs e)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                // Look up by Name — the only value Session guarantees
                cmd.CommandText =
                    "SELECT Name, Email, Role FROM Users WHERE Name = @name";
                cmd.Parameters.AddWithValue("@name", Session.CurrentUserName);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["Name"]?.ToString() ?? string.Empty;
                    txtEmail.Text = reader["Email"]?.ToString() ?? string.Empty;
                    lblRoleValue.Text = reader["Role"]?.ToString() ?? "—";
                }
                else
                {
                    MessageBox.Show("Could not find your user record.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load profile: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        // ── Save ──────────────────────────────────────────────────────────────────────
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var password = txtNewPassword.Text;
            var confirm = txtConfirmPassword.Text;

            // ── Validation ────────────────────────────────────────────────────────────
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name cannot be empty.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
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

            // ── Persist ───────────────────────────────────────────────────────────────
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();

                // Always match on the current name stored in Session
                if (string.IsNullOrEmpty(password))
                {
                    cmd.CommandText =
                        "UPDATE Users SET Name = @newName, Email = @email " +
                        "WHERE Name = @currentName";
                }
                else
                {
                    cmd.CommandText =
                        "UPDATE Users SET Name = @newName, Email = @email, Password = @pass " +
                        "WHERE Name = @currentName";
                    cmd.Parameters.AddWithValue("@pass", password);
                }

                cmd.Parameters.AddWithValue("@newName", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@currentName", Session.CurrentUserName);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    // Keep session in sync so greetings update immediately
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
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show(
                    "That email address is already in use by another account.\n" +
                    "Please choose a different email.",
                    "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update profile: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Cancel — close without saving ─────────────────────────────────────────────
        private void BtnCancel_Click(object? sender, EventArgs e) => this.Close();
    }
}