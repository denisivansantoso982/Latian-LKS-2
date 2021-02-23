using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class Employee : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt, dtJob;
        SqlCommand cmd;
        EncryptModel encryptModel = new EncryptModel();
        string imageName, imageUrl;
        int id;

        public Employee()
        {
            InitializeComponent();
            loadDataJob();
            loadDataEmployee();
            hide();
            dateTimePicker.MaxDate = DateTime.Now;
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
            textBox3.Text = "";
            textBox4.Text = "";
            richTextBox1.Text = "";
            pictureBox.Image = null;
            id = 0;
            pictureBox.Image = null;

            button3.Visible = false;
            button4.Visible = false;
        }

        void loadDataEmployee()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM Employee ORDER BY JobID ASC", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridEmployee.DataSource = dt;
                gridEmployee.Columns[0].Visible = false;
                gridEmployee.Columns[2].Visible = false;
                gridEmployee.Columns[5].Visible = false;
                gridEmployee.Columns[6].Visible = false;
                gridEmployee.Columns[8].Visible = false;
                gridEmployee.Columns[9].Visible = false;

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDataJob()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM Job", connection);
                dtJob = new DataTable();
                adapter.Fill(dtJob);

                comboBox.DataSource = dtJob;
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "ID";

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Employee_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox.Image, typeof(byte[]));

                    int job = (int) comboBox.SelectedValue;
                    string password = encryptModel.encrypt(textBox2.Text);


                    cmd = new SqlCommand("INSERT INTO Employee VALUES('" + textBox1.Text + "', '" + password + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + richTextBox1.Text + "', @DOB, " + job + ", '" + imageName + "', @photoImage)", connection);
                    cmd.Parameters.AddWithValue("@DOB", SqlDbType.Date).Value = dateTimePicker.Value;
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Employee added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataEmployee();
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox.Image, typeof(byte[]));

                    int job = (int) comboBox.SelectedValue;
                    string password = encryptModel.encrypt(textBox2.Text);


                    cmd = new SqlCommand("UPDATE Employee SET Username = '" + textBox1.Text + "', Password = '" + password + "', Name = '" + textBox3.Text + "', Email = '" + textBox4.Text + "', Address = '" + richTextBox1.Text + "', DateOfBirth = @DOB, JobID = " + job + ", Photo = '" + imageName + "', PhotoImage = @photoImage WHERE ID = " + id, connection);
                    cmd.Parameters.AddWithValue("@DOB", SqlDbType.Date).Value = dateTimePicker.Value;
                    cmd.Parameters.AddWithValue("@photoImage", image);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Employee updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataEmployee();
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
                if ( fileDialog.ShowDialog() == DialogResult.OK )
                {
                    imageName = fileDialog.SafeFileName;
                    imageUrl = fileDialog.FileName;
                    pictureBox.Image = Image.FromFile(imageUrl);
                }
            }
        }

        private void gridEmployee_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string val = e.Value.ToString();

            switch ( val )
            {
                case "1":
                    e.Value = "Admin";
                    break;
                case "2":
                    e.Value = "Front Officer";
                    break;
            }
        }

        private void gridEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = (int) gridEmployee.SelectedRows[0].Cells[0].Value;
            textBox1.Text = gridEmployee.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = (string) gridEmployee.SelectedRows[0].Cells[3].Value;
            textBox4.Text = gridEmployee.SelectedRows[0].Cells[4].Value.ToString();
            richTextBox1.Text = gridEmployee.SelectedRows[0].Cells[5].Value.ToString();
            dateTimePicker.Value = Convert.ToDateTime(gridEmployee.SelectedRows[0].Cells[6].Value);
            comboBox.SelectedIndex = (int) gridEmployee.SelectedRows[0].Cells[7].Value - 1;
            string images = gridEmployee.SelectedRows[0].Cells[8].Value.ToString();
            byte[] photo = (byte[]) gridEmployee.SelectedRows[0].Cells[9].Value;
            imageName = images;
            MemoryStream stream = new MemoryStream(photo);
            pictureBox.Image = Image.FromStream(stream);

            show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to delete this Employee data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ( dialog == DialogResult.Yes )
                {
                    connection.Open();

                    cmd = new SqlCommand("DELETE FROM Employee WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Employee deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataEmployee();
                    hide();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            loadDataEmployee();
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBox5.Text);
            gridEmployee.DataSource = dv;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox.Checked )
            {
                textBox2.UseSystemPasswordChar = false;
            } else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        bool validation()
        {
            if ( textBox1.Text == "" )
            {
                return false;
            } else if ( textBox2.Text == "" )
            {
                return false;
            } else if ( textBox3.Text == "" )
            {
                return false;
            } else if ( textBox4.Text == "" )
            {
                return false;
            }else if ( richTextBox1.Text == "" )
            {
                return false;
            }  else if ( dateTimePicker.Value == null )
            {
                return false;
            }

            return true;
        }
    }
}
