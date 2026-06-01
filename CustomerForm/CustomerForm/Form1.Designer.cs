namespace CustomerForm
{
    partial class CustomerMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerMenu));
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblWallet = new System.Windows.Forms.Label();
            this.btnBrowseMenu = new System.Windows.Forms.Button();
            this.btnViewOrder = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.BackColor = System.Drawing.SystemColors.Control;
            this.lblWelcome.Location = new System.Drawing.Point(78, 121);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(68, 16);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome!";
            // 
            // lblWallet
            // 
            this.lblWallet.AutoSize = true;
            this.lblWallet.BackColor = System.Drawing.SystemColors.Control;
            this.lblWallet.Location = new System.Drawing.Point(78, 165);
            this.lblWallet.Name = "lblWallet";
            this.lblWallet.Size = new System.Drawing.Size(165, 16);
            this.lblWallet.TabIndex = 1;
            this.lblWallet.Text = "E-Wallet Balance :RM 0.00";
            // 
            // btnBrowseMenu
            // 
            this.btnBrowseMenu.Location = new System.Drawing.Point(71, 271);
            this.btnBrowseMenu.Name = "btnBrowseMenu";
            this.btnBrowseMenu.Size = new System.Drawing.Size(172, 40);
            this.btnBrowseMenu.TabIndex = 2;
            this.btnBrowseMenu.Text = "Browse menu";
            this.btnBrowseMenu.UseVisualStyleBackColor = true;
            this.btnBrowseMenu.Click += new System.EventHandler(this.btnBrowseMenu_Click);
            // 
            // btnViewOrder
            // 
            this.btnViewOrder.Location = new System.Drawing.Point(71, 332);
            this.btnViewOrder.Name = "btnViewOrder";
            this.btnViewOrder.Size = new System.Drawing.Size(172, 40);
            this.btnViewOrder.TabIndex = 3;
            this.btnViewOrder.Text = "View My Orders";
            this.btnViewOrder.UseVisualStyleBackColor = true;
            this.btnViewOrder.Click += new System.EventHandler(this.btnViewOrder_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(71, 392);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(172, 40);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update Profile";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(71, 520);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(172, 40);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Log out";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.AutoScroll = true;
            this.MainPanel.BackColor = System.Drawing.Color.Transparent;
            this.MainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MainPanel.Location = new System.Drawing.Point(275, 90);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(950, 620);
            this.MainPanel.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(57, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(201, 73);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // CustomerMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1333, 753);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnViewOrder);
            this.Controls.Add(this.btnBrowseMenu);
            this.Controls.Add(this.lblWallet);
            this.Controls.Add(this.lblWelcome);
            this.Name = "CustomerMenu";
            this.Text = "Gr8Food";
            this.Load += new System.EventHandler(this.CustomerMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblWallet;
        private System.Windows.Forms.Button btnBrowseMenu;
        private System.Windows.Forms.Button btnViewOrder;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

