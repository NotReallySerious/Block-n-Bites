using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CustomerForm
{
    public partial class CustomerMenu : Form
    {
        public CustomerMenu()
        {
            InitializeComponent();
        }

        private void btnBrowseMenu_Click(object sender, EventArgs e)
        {
            BrowseMenu uc = new BrowseMenu();
            addUserControl(uc);
        }
        private void addUserControl(UserControl userControl)
            {
                if (this.MainPanel.Controls.Count > 0)
                    this.MainPanel.Controls.RemoveAt(0);

                userControl.Dock = DockStyle.Fill;

                this.MainPanel.Controls.Add(userControl);
                this.MainPanel.Tag = userControl;
            }

        private void CustomerMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            ViewOrder uc = new ViewOrder();
            adduserControl(uc);
        }
        private void adduserControl(UserControl userControl)
        {
            if (this.MainPanel.Controls.Count > 0)
                this.MainPanel.Controls.RemoveAt(0);

            userControl.Dock = DockStyle.Fill;

            this.MainPanel.Controls.Add(userControl);
            this.MainPanel.Tag = userControl;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateProfile uc = new UpdateProfile();
            addUserControl(uc);
        }

        private void AddUserControl(UserControl userControl)
        {
            if (this.MainPanel.Controls.Count > 0)
                this.MainPanel.Controls.RemoveAt(0);

            userControl.Dock = DockStyle.Fill;

            this.MainPanel.Controls.Add(userControl);
            this.MainPanel.Tag = userControl;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout uc = new Logout();
            uc.Show();
        }

        private void AdduserControl(UserControl userControl)
        {
            if (this.MainPanel.Controls.Count > 0)
                this.MainPanel.Controls.RemoveAt(0);

            userControl.Dock = DockStyle.Fill;

            this.MainPanel.Controls.Add(userControl);
            this.MainPanel.Tag = userControl;
        }
    }
}
