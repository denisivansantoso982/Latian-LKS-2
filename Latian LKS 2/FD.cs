using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class FD : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        string imageName, imageUrl;
        int id;

        public FD()
        {
            InitializeComponent();
            loadDataComboBox();
            loadDataFD();
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
            id = 0;
            pictureBox.Image = null;

            button3.Visible = false;
            button4.Visible = false;
        }

        void loadDataComboBox()
        {
            string[] dataCombo = { "Food", "Drink" };
            comboBox1.DataSource = dataCombo;
        }

        void loadDataFD()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM FoodsAndDrinks ORDER BY Type DESC", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridFD.DataSource = dt;
                gridFD.Columns[0].Visible = false;
                gridFD.Columns[4].Visible = false;
                gridFD.Columns[5].Visible = false;

                gridFD.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridFD.Columns[3].CellTemplate.Style.Font = new Font(gridFD.Font, FontStyle.Bold);

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

        private void FD_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void gridRoomType_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string value = e.Value.ToString();

            switch ( value )
            {
                case "F":
                    e.Value = "Food";
                    break;
                case "D":
                    e.Value = "Drink";
                    break;
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
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox.Image, typeof(byte[]));

                    string type = comboBox1.Text.Substring(0, 1);

                    cmd = new SqlCommand("INSERT INTO FoodsAndDrinks VALUES('" + textBox1.Text + "', '" + type + "', " + Convert.ToInt32(textBox2.Text) + ", '" + imageName + "', @photoImage)", connection);
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Food/Drink added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataFD();
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
            using ( OpenFileDialog fileDialog = new OpenFileDialog() )
            {
                if ( fileDialog.ShowDialog() == DialogResult.OK )
                {
                    imageName = fileDialog.SafeFileName;
                    imageUrl = fileDialog.FileName;
                    pictureBox.Image = Image.FromFile(imageUrl);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox.Image, typeof(byte[]));

                    string type = comboBox1.Text.Substring(0, 1);

                    cmd = new SqlCommand("UPDATE FoodsAndDrinks SET Name = '" + textBox1.Text + "', Type = '" + type + "', Price = " + Convert.ToInt32(textBox2.Text) + ", Photo = '" + imageName + "', PhotoImage = @photoImage WHERE ID = " + id, connection);
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Food/Drink updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataFD();
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
                DialogResult dialog = MessageBox.Show("Are you sure to delete this Item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ( dialog == DialogResult.Yes )
                {
                    connection.Open();

                    cmd = new SqlCommand("DELETE FROM FoodsAndDrinks WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Food/Drink deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataFD();
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
            textBox1.Text = gridFD.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = gridFD.SelectedRows[0].Cells[3].Value.ToString();
            imageName = gridFD.SelectedRows[0].Cells[4].Value.ToString();
            id = (int) gridFD.SelectedRows[0].Cells[0].Value;

            byte[] photo = (byte[]) gridFD.SelectedRows[0].Cells[5].Value;
            MemoryStream stream = new MemoryStream(photo);
            pictureBox.Image = Image.FromStream(stream);

            string type = gridFD.SelectedRows[0].Cells[2].Value.ToString();

            switch ( type )
            {
                case "Food":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "F":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "Drink":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "D":
                    comboBox1.SelectedIndex = 1;
                    break;
            }

            show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadDataFD();
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBox3.Text);
            gridFD.DataSource = dv;
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
            else if ( pictureBox.Image == null )
            {
                return false;
            }

            return true;
        }
    }
}
