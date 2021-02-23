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
    public partial class Room : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt, dtType;
        SqlCommand cmd;
        int id;

        public Room()
        {
            InitializeComponent();
            loadDataComboBox();
            loadDataRoom();
            this.BackColor = ColourModel.primary;
            hide();
        }

        void show()
        {
            button3.Visible = true;
            button4.Visible = true;
        }

        void hide()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = 0;
            richTextBox.Text = "";
            id = 0;

            button3.Visible = false;
            button4.Visible = false;
        }

        void loadDataComboBox()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT ID, Name FROM RoomType", connection);
                dtType = new DataTable();
                adapter.Fill(dtType);

                comboBox1.DataSource = dtType;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "ID";

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDataRoom()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select Room.ID, Name, RoomNumber, RoomFloor, Description, RoomPrice, RoomTypeID from Room inner join RoomType on Room.RoomTypeID = RoomType.ID", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridRoom.DataSource = dt;
                gridRoom.Columns[0].Visible = false;
                gridRoom.Columns[4].Visible = false;
                gridRoom.Columns[6].Visible = false;

                gridRoom.Columns[5].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridRoom.Columns[5].CellTemplate.Style.Font = new Font(gridRoom.Font, FontStyle.Bold);

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Room_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    cmd = new SqlCommand("INSERT INTO Room VALUES(" + Convert.ToInt32(comboBox1.SelectedValue) + ", " + Convert.ToInt32(textBox2.Text) + ", " + Convert.ToInt32(textBox1.Text) + ", '" + richTextBox.Text + "')", connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataRoom();
                    hide();
                }
                else
                {
                    MessageBox.Show("All forms are required!");
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to delete this Room?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ( dialog == DialogResult.Yes )
                {
                    connection.Open();

                    cmd = new SqlCommand("DELETE FROM Room WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataRoom();
                    hide();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    cmd = new SqlCommand("UPDATE Room SET RoomTypeID = " + Convert.ToInt32(comboBox1.SelectedValue) + ", RoomNumber = " + Convert.ToInt32(textBox2.Text) + ", RoomFloor = " + Convert.ToInt32(textBox1.Text) + ", Description = '" + richTextBox.Text + "' WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataRoom();
                }
                else
                {
                    MessageBox.Show("All forms are required!");
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = (int) gridRoom.SelectedRows[0].Cells[0].Value;
            comboBox1.SelectedValue = (int) gridRoom.SelectedRows[0].Cells[6].Value;
            textBox2.Text = gridRoom.SelectedRows[0].Cells[2].Value.ToString();
            textBox1.Text = gridRoom.SelectedRows[0].Cells[3].Value.ToString();
            richTextBox.Text = gridRoom.SelectedRows[0].Cells[4].Value.ToString();

            show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadDataRoom();
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBox3.Text);
            gridRoom.DataSource = dv;
        }

        bool validation()
        {
            if ( textBox1.Text == "" )
            {
                return false;
            }
            else if ( textBox2.Text == "" )
            {
                return false;
            }
            else if ( richTextBox.Text == "" )
            {
                return false;
            }

            return true;
        }
    }
}
