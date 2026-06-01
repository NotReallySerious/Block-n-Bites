using System.Drawing;
using System.Windows.Forms;

namespace Gr8Food
{
    partial class Profile
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            grpProfile = new GroupBox();
            lblNameField = new Label();
            txtName = new TextBox();
            lblEmailField = new Label();
            txtEmail = new TextBox();
            lblRoleField = new Label();
            lblRoleValue = new Label();
            lblNewPwdField = new Label();
            txtNewPassword = new TextBox();
            lblConfirmPwd = new Label();
            txtConfirmPassword = new TextBox();
            lblPwdHint = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            lblTitle = new Label();
            grpProfile.SuspendLayout();
            SuspendLayout();
            // 
            // grpProfile
            // 
            grpProfile.BackColor = Color.Transparent;
            grpProfile.Controls.Add(lblNameField);
            grpProfile.Controls.Add(txtName);
            grpProfile.Controls.Add(lblEmailField);
            grpProfile.Controls.Add(txtEmail);
            grpProfile.Controls.Add(lblRoleField);
            grpProfile.Controls.Add(lblRoleValue);
            grpProfile.Controls.Add(lblNewPwdField);
            grpProfile.Controls.Add(txtNewPassword);
            grpProfile.Controls.Add(lblConfirmPwd);
            grpProfile.Controls.Add(txtConfirmPassword);
            grpProfile.Controls.Add(lblPwdHint);
            grpProfile.Controls.Add(btnSave);
            grpProfile.Controls.Add(btnCancel);
            grpProfile.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            grpProfile.Location = new Point(56, 181);
            grpProfile.Margin = new Padding(6, 6, 6, 6);
            grpProfile.Name = "grpProfile";
            grpProfile.Padding = new Padding(11, 13, 11, 13);
            grpProfile.Size = new Size(1189, 939);
            grpProfile.TabIndex = 1;
            grpProfile.TabStop = false;
            grpProfile.Text = "User Information";
            // 
            // lblNameField
            // 
            lblNameField.Font = new Font("Segoe UI", 9.75F);
            lblNameField.Location = new Point(37, 77);
            lblNameField.Margin = new Padding(6, 0, 6, 0);
            lblNameField.Name = "lblNameField";
            lblNameField.Size = new Size(1096, 51);
            lblNameField.TabIndex = 0;
            lblNameField.Text = "Name:";
            // 
            // txtName
            // 
            txtName.Font = new Font("Segoe UI", 9F);
            txtName.Location = new Point(37, 137);
            txtName.Margin = new Padding(6, 6, 6, 6);
            txtName.Name = "txtName";
            txtName.Size = new Size(1092, 39);
            txtName.TabIndex = 1;
            // 
            // lblEmailField
            // 
            lblEmailField.Font = new Font("Segoe UI", 9.75F);
            lblEmailField.Location = new Point(37, 230);
            lblEmailField.Margin = new Padding(6, 0, 6, 0);
            lblEmailField.Name = "lblEmailField";
            lblEmailField.Size = new Size(1096, 51);
            lblEmailField.TabIndex = 2;
            lblEmailField.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.Location = new Point(37, 290);
            txtEmail.Margin = new Padding(6, 6, 6, 6);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(1092, 39);
            txtEmail.TabIndex = 3;
            // 
            // lblRoleField
            // 
            lblRoleField.Font = new Font("Segoe UI", 9.75F);
            lblRoleField.Location = new Point(37, 384);
            lblRoleField.Margin = new Padding(6, 0, 6, 0);
            lblRoleField.Name = "lblRoleField";
            lblRoleField.Size = new Size(1096, 51);
            lblRoleField.TabIndex = 4;
            lblRoleField.Text = "Role:";
            // 
            // lblRoleValue
            // 
            lblRoleValue.Font = new Font("Segoe UI", 9.75F);
            lblRoleValue.ForeColor = Color.DimGray;
            lblRoleValue.Location = new Point(37, 444);
            lblRoleValue.Margin = new Padding(6, 0, 6, 0);
            lblRoleValue.Name = "lblRoleValue";
            lblRoleValue.Size = new Size(1096, 51);
            lblRoleValue.TabIndex = 5;
            lblRoleValue.Text = "—";
            // 
            // lblNewPwdField
            // 
            lblNewPwdField.Font = new Font("Segoe UI", 9.75F);
            lblNewPwdField.Location = new Point(37, 529);
            lblNewPwdField.Margin = new Padding(6, 0, 6, 0);
            lblNewPwdField.Name = "lblNewPwdField";
            lblNewPwdField.Size = new Size(1096, 51);
            lblNewPwdField.TabIndex = 6;
            lblNewPwdField.Text = "New Password:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 9F);
            txtNewPassword.Location = new Point(37, 589);
            txtNewPassword.Margin = new Padding(6, 6, 6, 6);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '●';
            txtNewPassword.Size = new Size(1092, 39);
            txtNewPassword.TabIndex = 7;
            // 
            // lblConfirmPwd
            // 
            lblConfirmPwd.Font = new Font("Segoe UI", 9.75F);
            lblConfirmPwd.Location = new Point(37, 683);
            lblConfirmPwd.Margin = new Padding(6, 0, 6, 0);
            lblConfirmPwd.Name = "lblConfirmPwd";
            lblConfirmPwd.Size = new Size(1096, 51);
            lblConfirmPwd.TabIndex = 8;
            lblConfirmPwd.Text = "Confirm New Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 9F);
            txtConfirmPassword.Location = new Point(37, 742);
            txtConfirmPassword.Margin = new Padding(6, 6, 6, 6);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Size = new Size(1092, 39);
            txtConfirmPassword.TabIndex = 9;
            // 
            // lblPwdHint
            // 
            lblPwdHint.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic);
            lblPwdHint.ForeColor = Color.DimGray;
            lblPwdHint.Location = new Point(37, 815);
            lblPwdHint.Margin = new Padding(6, 0, 6, 0);
            lblPwdHint.Name = "lblPwdHint";
            lblPwdHint.Size = new Size(1096, 47);
            lblPwdHint.TabIndex = 10;
            lblPwdHint.Text = "Leave New Password blank to keep your current password.";
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnSave.Location = new Point(37, 832);
            btnSave.Margin = new Padding(6, 6, 6, 6);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(334, 102);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnCancel.Location = new Point(405, 832);
            btnCancel.Margin = new Padding(6, 6, 6, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(334, 102);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.Location = new Point(56, 43);
            lblTitle.Margin = new Padding(6, 0, 6, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1189, 117);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Edit Profile";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmProfile
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Wheat;
            ClientSize = new Size(1300, 1195);
            Controls.Add(lblTitle);
            Controls.Add(grpProfile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6, 6, 6, 6);
            MaximizeBox = false;
            Name = "frmProfile";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Profile";
            grpProfile.ResumeLayout(false);
            grpProfile.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // ── Field declarations ────────────────────────────────────────────────────────
        private System.Windows.Forms.GroupBox grpProfile;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNameField;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblEmailField;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblRoleField;
        private System.Windows.Forms.Label lblRoleValue;
        private System.Windows.Forms.Label lblNewPwdField;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblConfirmPwd;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label lblPwdHint;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}