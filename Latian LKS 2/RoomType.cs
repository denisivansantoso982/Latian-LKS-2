using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class RoomType : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        string imageName, imageUrl;
        int id;

        public RoomType()
        {
            InitializeComponent();
            loadData();
            hide();
            this.BackColor = ColourModel.primary;
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
            numericUpDown1.Value = 1;
            pictureBox1.Image = null;
            id = 0;

            button3.Visible = false;
            button4.Visible = false;
        }

        void loadData()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM RoomType", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridRoomType.DataSource = dt;
                gridRoomType.Columns[0].Visible = false;
                gridRoomType.Columns[4].Visible = false;
                gridRoomType.Columns[5].Visible = false;

                gridRoomType.Columns[3].HeaderText = "Room Price";

                gridRoomType.Columns[2].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridRoomType.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridRoomType.Columns[3].CellTemplate.Style.Font = new Font(gridRoomType.Font, FontStyle.Bold);

                connection.Close();
            } catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RoomType_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                    cmd = new SqlCommand("INSERT INTO RoomType VALUES('" + textBox1.Text + "', " + Convert.ToInt32(numericUpDown1.Value) + ", " + Convert.ToInt32(textBox2.Text) + ", '" + imageName + "', @photoImage)", connection);
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadData();
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

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog() )
            {
                if (fileDialog.ShowDialog() == DialogResult.OK )
                {
                    imageName = fileDialog.SafeFileName;
                    imageUrl = fileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(imageUrl);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                    cmd = new SqlCommand("UPDATE RoomType SET Name = '" + textBox1.Text + "', Capacity = " + numericUpDown1.Value + ", RoomPrice = " + Convert.ToInt32(textBox2.Text) + ", Photo = '" + imageName + "', PhotoImage = @photoImage WHERE ID = " + id, connection);
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadData();
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
                DialogResult dialog = MessageBox.Show("Are you sure to delete this Room Type?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes )
                {
                    connection.Open();

                    cmd = new SqlCommand("DELETE FROM RoomType WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data room deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadData();
                    hide();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridRoomType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = gridRoomType.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDown1.Value = (int) gridRoomType.SelectedRows[0].Cells[2].Value;
            textBox2.Text = gridRoomType.SelectedRows[0].Cells[3].Value.ToString();
            imageName = gridRoomType.SelectedRows[0].Cells[4].Value.ToString();
            id = (int) gridRoomType.SelectedRows[0].Cells[0].Value;

            byte[] photo = (byte[]) gridRoomType.SelectedRows[0].Cells[5].Value;
            MemoryStream stream = new MemoryStream(photo);
            pictureBox1.Image = Image.FromStream(stream);

            show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hide();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadData();
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBox3.Text);
            gridRoomType.DataSource = dv;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string[] data = textBox1.Text.Split(' ');

        }

        bool validation()
        {
            if (textBox1.Text == "" )
            {
                return false;
            } else if ( textBox2.Text == "" )
            {
                return false;
            } else if ( numericUpDown1.Value < 1 )
            {
                return false;
            } else if (pictureBox1.Image == null )
            {
                return false;
            }

            return true;
        }
    }
}
