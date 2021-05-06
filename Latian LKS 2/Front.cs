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
    public partial class Front : Form
    {
        public Front()
        {
            InitializeComponent();
            this.BackColor = ColourModel.primary;
            string name = EmployeeModel.Name;
            var first = name.IndexOf(" ") > -1 ? name.Substring(0, name.IndexOf(" ")) : name;
            labelName.Text = first.ToString();

            panelReservation.BackColor = ColourModel.glass;
            panelCheckIn.BackColor = ColourModel.glass;
            panelRequest.BackColor = ColourModel.glass;
            panelReportCheckIn.BackColor = ColourModel.glass;
            panelReportGuest.BackColor = ColourModel.glass;
            panelCheckOut.BackColor = ColourModel.glass;

            labelDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panelLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Apakah anda yakin?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if ( dialog == DialogResult.Yes )
            {
                this.Close();
                Login login = new Login();
                login.Show();
            }
            else
            { }
        }

        private void panelReservation_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            reservation.ShowDialog();
        }

        private void panelCheckIn_Click(object sender, EventArgs e)
        {
            CheckIn checkIn = new CheckIn();
            checkIn.ShowDialog();
        }

        private void panelRequest_Click(object sender, EventArgs e)
        {
            RequestItem request = new RequestItem();
            request.ShowDialog();
        }

        private void panelCheckOut_Click(object sender, EventArgs e)
        {
            CheckOut checkOut = new CheckOut();
            checkOut.ShowDialog();
        }

        private void panelReportGuest_Click(object sender, EventArgs e)
        {
            ReportGuest reportGuest = new ReportGuest();
            reportGuest.ShowDialog();
        }

        private void panelReportCheckIn_Click(object sender, EventArgs e)
        {
            ReportCheckIn reportCheckIn = new ReportCheckIn();
            reportCheckIn.ShowDialog();
        }

        private void Front_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColor(this.ClientRectangle, e);
        }
    }
}
