namespace Gr8Food
{
    partial class ChefManageOrders
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
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            this.lblOrders = new System.Windows.Forms.Label();
            this.lstOrders = new System.Windows.Forms.ListBox();
            this.cboSelectedOrder = new System.Windows.Forms.ComboBox();
            this.lblSelectedOrder = new System.Windows.Forms.Label();
            this.lblOrderDetails = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpdateStatus
            // 
            this.btnUpdateStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateStatus.Location = new System.Drawing.Point(22, 412);
            this.btnUpdateStatus.Name = "btnUpdateStatus";
            this.btnUpdateStatus.Size = new System.Drawing.Size(253, 52);
            this.btnUpdateStatus.TabIndex = 5;
            this.btnUpdateStatus.Text = "Update Status";
            this.btnUpdateStatus.UseVisualStyleBackColor = true;
            this.btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);
            // 
            // lblOrders
            // 
            this.lblOrders.AutoSize = true;
            this.lblOrders.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrders.Location = new System.Drawing.Point(36, 21);
            this.lblOrders.Name = "lblOrders";
            this.lblOrders.Size = new System.Drawing.Size(293, 64);
            this.lblOrders.TabIndex = 4;
            this.lblOrders.Text = "All Orders:";
            // 
            // lstOrders
            // 
            this.lstOrders.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstOrders.FormattingEnabled = true;
            this.lstOrders.ItemHeight = 37;
            this.lstOrders.Location = new System.Drawing.Point(47, 105);
            this.lstOrders.Name = "lstOrders";
            this.lstOrders.Size = new System.Drawing.Size(839, 189);
            this.lstOrders.TabIndex = 3;
            // 
            // cboSelectedOrder
            // 
            this.cboSelectedOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedOrder.FormattingEnabled = true;
            this.cboSelectedOrder.Location = new System.Drawing.Point(269, 332);
            this.cboSelectedOrder.Name = "cboSelectedOrder";
            this.cboSelectedOrder.Size = new System.Drawing.Size(271, 40);
            this.cboSelectedOrder.TabIndex = 7;
            this.cboSelectedOrder.SelectedIndexChanged += new System.EventHandler(this.cboSelectedOrder_SelectedIndexChanged);
            // 
            // lblSelectedOrder
            // 
            this.lblSelectedOrder.AutoSize = true;
            this.lblSelectedOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedOrder.Location = new System.Drawing.Point(24, 332);
            this.lblSelectedOrder.Name = "lblSelectedOrder";
            this.lblSelectedOrder.Size = new System.Drawing.Size(239, 37);
            this.lblSelectedOrder.TabIndex = 6;
            this.lblSelectedOrder.Text = "Selected Order:";
            // 
            // lblOrderDetails
            // 
            this.lblOrderDetails.AutoSize = true;
            this.lblOrderDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderDetails.Location = new System.Drawing.Point(573, 335);
            this.lblOrderDetails.Name = "lblOrderDetails";
            this.lblOrderDetails.Size = new System.Drawing.Size(205, 37);
            this.lblOrderDetails.TabIndex = 8;
            this.lblOrderDetails.Text = "Order Details";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(392, 412);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(148, 52);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ChefManageOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 619);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblOrderDetails);
            this.Controls.Add(this.cboSelectedOrder);
            this.Controls.Add(this.lblSelectedOrder);
            this.Controls.Add(this.btnUpdateStatus);
            this.Controls.Add(this.lblOrders);
            this.Controls.Add(this.lstOrders);
            this.Name = "ChefManageOrders";
            this.Text = "ChefManageOrders";
            this.Load += new System.EventHandler(this.ChefManageOrders_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateStatus;
        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.ListBox lstOrders;
        private System.Windows.Forms.ComboBox cboSelectedOrder;
        private System.Windows.Forms.Label lblSelectedOrder;
        private System.Windows.Forms.Label lblOrderDetails;
        private System.Windows.Forms.Button btnRefresh;
    }
}