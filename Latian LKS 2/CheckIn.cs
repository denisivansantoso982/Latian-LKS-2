using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class CheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        SqlDataReader reader;
        int id = 0;

        public CheckIn()
        {
            InitializeComponent();
            loadDataGender();
            loadDataRoom();
            this.BackColor = ColourModel.primary;
        }

        void loadDataGender()
        {
            string[] gender = { "Male", "Female" };
            comboBoxGender.DataSource = gender;
        }

        void loadDataRoom()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select Reservation.ID, BookingCode, RoomFloor, RoomNumber, RoomType.Name, StartDateTime from Reservation inner join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID inner join Room on ReservationRoom.RoomID = Room.ID inner join RoomType on Room.RoomTypeID = RoomType.ID where BookingCode = '" + textBoxSearch.Text + "' and CheckInDateTime > GETDATE()", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridRoom.DataSource = dt;
                gridRoom.Columns[0].Visible = false;
                gridRoom.Columns[1].Visible = false;

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckIn_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select Reservation.ID, BookingCode, RoomFloor, RoomNumber, RoomType.Name, StartDateTime from Reservation inner join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID inner join Room on ReservationRoom.RoomID = Room.ID inner join RoomType on Room.RoomTypeID = RoomType.ID where BookingCode = '" + textBoxSearch.Text + "' and CheckInDateTime > GETDATE()", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridRoom.DataSource = dt;

                if (dt.Rows.Count < 1 )
                {
                    MessageBox.Show("Room not found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    int id = Convert.ToInt32(gridRoom.SelectedRows[0].Cells[0].Value);
                    cmd = new SqlCommand("select * from Customer inner join Reservation on Customer.ID = Reservation.CustomerID where Reservation.ID = " + id, connection);
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    id = Convert.ToInt32(reader["ID"]);
                    textBox1.Text = Convert.ToString(reader["PhoneNumber"]);
                    textBox2.Text = Convert.ToString(reader["Name"]);
                    textBox3.Text = Convert.ToString(reader["Email"]);
                    textBox4.Text = Convert.ToString(reader["Age"]);
                    textBox5.Text = Convert.ToString(reader["NIK"]);

                    string gend = Convert.ToString(reader["Gender"]);
                    if ( gend == "M" )
                        comboBoxGender.SelectedIndex = 0;
                    else if ( gend == "F" )
                        comboBoxGender.SelectedIndex = 1;
                }

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridRoom.RowCount < 1 || id == 0 )
                {
                    MessageBox.Show("Please select and fill the customer form!");

                    connection.Open();

                    foreach (DataGridViewRow row in gridRoom.Rows )
                    {
                        int idRes = Convert.ToInt32(row.Cells[0].Value);
                        cmd = new SqlCommand("UPDATE ReservationRoom SET CheckInDateTime = GETDATE(), StartDateTime = GETDATE() WHERE ReservationID = " + idRes, connection);
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                } else
                {

                }
            } catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            comboBoxGender.Enabled = false;
            id = 0;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                cmd = new SqlCommand("select * from Customer where PhoneNumber  = '" + textBox1.Text + "'", connection);
                reader = cmd.ExecuteReader();
                reader.Read();

                if ( reader.HasRows )
                {
                    id = Convert.ToInt32(reader["ID"]);
                    textBox2.Text = Convert.ToString(reader["Name"]);
                    textBox3.Text = Convert.ToString(reader["Email"]);
                    textBox4.Text = Convert.ToString(reader["Age"]);
                    textBox5.Text = Convert.ToString(reader["NIK"]);

                    string gend = Convert.ToString(reader["Gender"]);
                    if ( gend == "M" )
                        comboBoxGender.SelectedIndex = 0;
                    else if ( gend == "F" )
                        comboBoxGender.SelectedIndex = 1;

                } else
                {
                    MessageBox.Show("Customer not found! You can fill the customer form now!");
                    textBox2.ReadOnly = false;
                    textBox3.ReadOnly = false;
                    textBox4.ReadOnly = false;
                    textBox5.ReadOnly = false;
                    comboBoxGender.Enabled = true;

                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    id = 0;
                }

                reader.Close();
                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
