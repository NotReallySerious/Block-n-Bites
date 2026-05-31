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
    public partial class Logincs : Form
    {
        public Logincs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Chef newChef = new Chef("2","Ahmad", "test@email.com");
            MainChef newMainChef = new MainChef(newChef);
            newMainChef.Show();
            this.Hide();
        }

        private void Logincs_Load(object sender, EventArgs e)
        {

        }
    }
}
