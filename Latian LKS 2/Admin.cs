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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            this.BackColor = ColourModel.primary;
            string name = EmployeeModel.Name;
            var first = name.IndexOf(" ") > -1 ? name.Substring(0, name.IndexOf(" ")) : name;
            labelName.Text = first.ToString();

            panelEmployee.BackColor = ColourModel.glass;
            panelFD.BackColor = ColourModel.glass;
            panelItem.BackColor = ColourModel.glass;
            panelRoom.BackColor = ColourModel.glass;
            panelRoomType.BackColor = ColourModel.glass;

            labelDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            button1.BackColor = ColourModel.primary;
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

        private void panelEmployee_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.ShowDialog();
        }

        private void panelItem_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.ShowDialog();
        }

        private void panelFD_Click(object sender, EventArgs e)
        {
            FD fd = new FD();
            fd.ShowDialog();
        }

        private void panelRoomType_Click(object sender, EventArgs e)
        {
            RoomType roomType = new RoomType();
            roomType.ShowDialog();
        }

        private void panelRoom_Click(object sender, EventArgs e)
        {
            Room room = new Room();
            room.ShowDialog();
        }

        private void Admin_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColorForMain(this.ClientRectangle, e);
        }

        private void panelEmployee_MouseLeave(object sender, EventArgs e)
        {
            panelEmployee.BackColor = ColourModel.glass;
        }

        private void panelEmployee_MouseEnter(object sender, EventArgs e)
        {
            panelEmployee.BackColor = ColourModel.glassHover;
        }

        private void panelItem_MouseLeave(object sender, EventArgs e)
        {
            panelItem.BackColor = ColourModel.glass;
        }

        private void panelItem_MouseEnter(object sender, EventArgs e)
        {
            panelItem.BackColor = ColourModel.glassHover;
        }

        private void panelFD_MouseEnter(object sender, EventArgs e)
        {
            panelFD.BackColor = ColourModel.glassHover;
        }

        private void panelFD_MouseLeave(object sender, EventArgs e)
        {
            panelFD.BackColor = ColourModel.glass;
        }

        private void panelRoomType_MouseEnter(object sender, EventArgs e)
        {
            panelRoomType.BackColor = ColourModel.glassHover;
        }

        private void panelRoomType_MouseLeave(object sender, EventArgs e)
        {
            panelRoomType.BackColor = ColourModel.glass;
        }

        private void panelRoom_MouseEnter(object sender, EventArgs e)
        {
            panelRoom.BackColor = ColourModel.glassHover;
        }

        private void panelRoom_MouseLeave(object sender, EventArgs e)
        {
            panelRoom.BackColor = ColourModel.glass;
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
            Front front = new Front();
            front.ShowInTaskbar = false;
            front.ShowDialog();
            this.Close();
        }

        private void panelMaster_MouseEnter(object sender, EventArgs e)
        {
            panelMaster.BackColor = ColourModel.glassHover;
            label16.BackColor = Color.Transparent;
        }

        private void panelMaster_MouseLeave(object sender, EventArgs e)
        {
            panelMaster.BackColor = Color.Transparent;
        }
    }
}
