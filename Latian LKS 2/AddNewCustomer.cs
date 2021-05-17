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
    public partial class AddNewCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlCommand cmd;

        public AddNewCustomer()
        {
            InitializeComponent();
            loadDataGender();
            button1.BackColor = ColourModel.primary;
            button2.BackColor = ColourModel.primary;
        }

        void loadDataGender()
        {
            string[] gender = { "Male", "Female" };
            comboBox1.DataSource = gender;
        }

        private void AddNewCustomer_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColor(this.ClientRectangle, e);
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

        bool validation()
        {
            if (textBox1.Text == "" )
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
            } else if ( numericUpDown1.Value == 0 )
            {
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    string gender = comboBox1.Text.Substring(0, 1);
                    cmd = new SqlCommand("INSERT INTO Customer VALUES('" + textBox1.Text + "', " + Convert.ToInt32(textBox2.Text) + ", '" + textBox3.Text + "', '" + gender + "', '" + textBox4.Text + "', " + numericUpDown1.Value + ")", connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Customer added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
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
    }
}
