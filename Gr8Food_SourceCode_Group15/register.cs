using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Gr8Food_SourceCode_Group15
{
    public partial class frmregister : Form
    {
        public frmregister()
        {
            InitializeComponent();
            btnRegister.Click += BtnRegister_Click;
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required. The registration will restart.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // clear fields and restart
                txtName.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtName.Focus();
                return;
            }

            // validate email format
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    throw new FormatException("Invalid email");
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // validate password strength: at least 12 chars, one digit, one upper, one lower, one special
            var pwd = password;
            if (pwd.Length < 12 || !pwd.Any(char.IsDigit) || !pwd.Any(char.IsUpper) || !pwd.Any(char.IsLower) || !pwd.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Password must be at least 12 characters and include at least one digit, one uppercase letter, one lowercase letter, and one special character.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
                return;
            }

            try
            {
                var ok = Database.RegisterUser(name, email, password, "Customer");
                if (ok)
                {
                    MessageBox.Show("Registration successful. You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Session.LastRegisteredEmail = email;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed. Email might already be in use.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // keep the registration form open to allow correction
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
