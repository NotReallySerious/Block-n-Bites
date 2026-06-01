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

namespace Gr8Food
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            this.Load += FrmLogin_Load;
        }

        private void FrmLogin_Load(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session.LastRegisteredEmail))
            {
                txtName.Text = Session.LastRegisteredEmail;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            // Signal Program to open the registration form and close this login dialog
            this.Tag = "OpenRegister";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            var username = txtName.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password." , "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var role = Database.AuthenticateUser(username, password);
                if (role == null)
                {
                    // diagnostic: check if user exists in DB and show connection info
                    var connInfo = Database.GetConnectionStringForDiagnostics();
                    var row = Database.FindUserRecord(username);
                    if (row == null)
                    {
                        MessageBox.Show("Login failed: user not found in database.\nConnection: " + connInfo, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Login failed: user exists but password did not match.\nConnection: " + connInfo, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                // pass role back to Program via Tag and close dialog with OK
                this.Tag = role;
                // store the logged-in username for other forms
                Session.CurrentUserName = username;
                this.DialogResult = DialogResult.OK;
                this.Close();
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
