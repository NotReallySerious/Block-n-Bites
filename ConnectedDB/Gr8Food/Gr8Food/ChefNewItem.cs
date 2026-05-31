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
    public partial class ChefNewItem : Form
    {
        Chef loggedinchef;
        public ChefNewItem(Chef newchef)
        {
            InitializeComponent();
            loggedinchef = newchef;
        }

        private void ChefNewItem_Load(object sender, EventArgs e)
        {
            cboCategory.Items.Add("Breakfast");
            cboCategory.Items.Add("Lunch");
            cboCategory.Items.Add("Dinner");
            cboCategory.Items.Add("Snacks");
            cboCategory.Items.Add("Drinks");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (txtItemName.Text == "" || txtPrice.Text == "" ||
                cboCategory.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            //  validate price
            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("Price must be a valid number greater than 0.");
                return;
            }
            loggedinchef.AddMenuItem(txtItemName.Text, cboCategory.Text, price);
            MessageBox.Show("Item Sucessfully Added");
            this.Close();
        }
    }
}
