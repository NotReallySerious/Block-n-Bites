using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gr8Food
{
    public partial class MainChef : Form
    {
        Chef loggedinchef;
        public MainChef(Chef newChef)
        {
            loggedinchef = newChef;
            InitializeComponent();
        }

        private void MainChef_Load(object sender, EventArgs e)
        {
            Chef newChef = new Chef();
            lblWelcome.Text = $"Welcome, {loggedinchef.ChefName}";
            foreach (string item in loggedinchef.LoadOrders())
            {
                lstMenu.Items.Add(item);
            }
        }

        private void btnEditMenu_Click(object sender, EventArgs e)
        {
            ChefMenu newChefMenu = new ChefMenu(loggedinchef);
            newChefMenu.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnManageOrder_Click(object sender, EventArgs e)
        {
            ChefManageOrders newChefManageOrders = new ChefManageOrders(loggedinchef);
            newChefManageOrders.Show();
        }

        private void btnChangeProfile_Click(object sender, EventArgs e)
        {
            ChefUpdateProfile newChefUpdateProfile = new ChefUpdateProfile(loggedinchef);
            newChefUpdateProfile.Show();
        }
    }
}
