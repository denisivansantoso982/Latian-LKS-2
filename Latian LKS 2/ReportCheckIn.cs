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
    public partial class ReportCheckIn : Form
    {
        public ReportCheckIn()
        {
            InitializeComponent();
            this.BackColor = ColourModel.primary;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportCheckIn_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }
    }
}
