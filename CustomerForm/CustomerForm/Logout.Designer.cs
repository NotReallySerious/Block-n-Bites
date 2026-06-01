namespace CustomerForm
{
    partial class Logout
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logout));
            this.lblDoublesure = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDoublesure
            // 
            this.lblDoublesure.AutoSize = true;
            this.lblDoublesure.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDoublesure.Location = new System.Drawing.Point(224, 147);
            this.lblDoublesure.Name = "lblDoublesure";
            this.lblDoublesure.Size = new System.Drawing.Size(453, 32);
            this.lblDoublesure.TabIndex = 0;
            this.lblDoublesure.Text = "Are You Sure You Want to Log Out";
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(267, 267);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(520, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "No";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Logout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblDoublesure);
            this.Name = "Logout";
            this.Size = new System.Drawing.Size(1008, 644);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDoublesure;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button button2;
    }
}
