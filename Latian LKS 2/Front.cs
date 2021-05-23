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
            button1.BackColor = ColourModel.primary;

            if (EmployeeModel.JobID == 1)
            {
                panelMaster.Visible = true;
                label1.Text = "Admin";
            }
            else
            {
                panelMaster.Visible = false;
                label1.Text = "Front Officer";
            }
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
            GradientModel.gradientColorForMain(this.ClientRectangle, e);
        }

        private void panelReservation_MouseEnter(object sender, EventArgs e)
        {
            panelReservation.BackColor = ColourModel.glassHover;
        }

        private void panelReservation_MouseLeave(object sender, EventArgs e)
        {
            panelReservation.BackColor = ColourModel.glass;
        }

        private void panelCheckIn_MouseEnter(object sender, EventArgs e)
        {
            panelCheckIn.BackColor = ColourModel.glassHover;
        }

        private void panelCheckIn_MouseLeave(object sender, EventArgs e)
        {
            panelCheckIn.BackColor = ColourModel.glass;
        }

        private void panelRequest_MouseEnter(object sender, EventArgs e)
        {
            panelRequest.BackColor = ColourModel.glassHover;
        }

        private void panelRequest_MouseLeave(object sender, EventArgs e)
        {
            panelRequest.BackColor = ColourModel.glass;
        }

        private void panelCheckOut_MouseEnter(object sender, EventArgs e)
        {
            panelCheckOut.BackColor = ColourModel.glassHover;
        }

        private void panelCheckOut_MouseLeave(object sender, EventArgs e)
        {
            panelCheckOut.BackColor = ColourModel.glass;
        }

        private void panelReportGuest_MouseEnter(object sender, EventArgs e)
        {
            panelReportGuest.BackColor = ColourModel.glassHover;
        }

        private void panelReportGuest_MouseLeave(object sender, EventArgs e)
        {
            panelReportGuest.BackColor = ColourModel.glass;
        }

        private void panelReportCheckIn_MouseEnter(object sender, EventArgs e)
        {
            panelReportCheckIn.BackColor = ColourModel.glassHover;
        }

        private void panelReportCheckIn_MouseLeave(object sender, EventArgs e)
        {
            panelReportCheckIn.BackColor = ColourModel.glass;
        }

        private void panelLogout_MouseEnter(object sender, EventArgs e)
        {
            panelLogout.BackColor = ColourModel.glassHover;
            label14.BackColor = Color.Transparent;
        }

        private void panelLogout_MouseLeave(object sender, EventArgs e)
        {
            panelLogout.BackColor = Color.Transparent;
        }

        private void panelMaster_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin();
            admin.Show();
        }

        private void panelMaster_MouseEnter(object sender, EventArgs e)
        {
            panelMaster.BackColor = ColourModel.glassHover;
            label13.BackColor = Color.Transparent;
        }

        private void panelMaster_MouseLeave(object sender, EventArgs e)
        {
            panelMaster.BackColor = Color.Transparent;
        }
    }
}
