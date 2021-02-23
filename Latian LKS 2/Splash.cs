using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class Splash : Form
    {
        int loading = 0;
        public Splash()
        {
            InitializeComponent();
            timer.Start();
            panel1.Width = 0;
            this.BackColor = ColourModel.primary;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            loading += 5;

            panel1.Width = loading;

            if (panel1.Width >= this.Width )
            {
                timer.Stop();
                Login login = new Login();
                login.Show();
                this.Hide();
            }
            else
            { }
        }
    }
}
