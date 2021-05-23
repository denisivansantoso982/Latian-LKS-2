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
    public partial class Login : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlCommand command;
        SqlDataReader reader;
        EncryptModel encryptModel = new EncryptModel();

        public Login()
        {
            InitializeComponent();
            button1.BackColor = ColourModel.primary;
            button2.BackColor = ColourModel.primary;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked )
            {
                textBox2.UseSystemPasswordChar = false;
            } else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsWhiteSpace(e.KeyChar);

            if (e.KeyChar == (char) Keys.Enter )
            {
                button2.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string password = encryptModel.encrypt(textBox2.Text);
                command = new SqlCommand("SELECT * FROM Employee WHERE Username = '" + textBox1.Text + "' AND Password = '" + password + "'", connection);
                reader = command.ExecuteReader();
                reader.Read();

                if ( reader.HasRows )
                {
                    EmployeeModel.ID = Convert.ToInt32(reader["ID"]);
                    EmployeeModel.Username = Convert.ToString(reader["Username"]);
                    EmployeeModel.Password = Convert.ToString(reader["Password"]);
                    EmployeeModel.Name = Convert.ToString(reader["Name"]);
                    EmployeeModel.Email = Convert.ToString(reader["Email"]);
                    EmployeeModel.Address = Convert.ToString(reader["Address"]);
                    EmployeeModel.Birth = Convert.ToDateTime(reader["DateOfBirth"]);
                    EmployeeModel.JobID = Convert.ToInt32(reader["JobID"]);
                    if (reader["Photo"] != DBNull.Value )
                    {
                        EmployeeModel.Photo = Convert.ToString(reader["Photo"]);
                        EmployeeModel.PhotoImage = (byte[]) reader["PhotoImage"];
                    }

                    Admin admin = new Admin();
                    Front front = new Front();

                    if (EmployeeModel.JobID == 1 )
                    {
                        front.ShowInTaskbar = false;
                        front.Show();
                        admin.Show();
                        admin.BringToFront();
                        this.Close();
                    } else
                    {
                        front.Show();
                        this.Close();
                    }
                } else
                {
                    MessageBox.Show("Account not registered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                connection.Close();
            } catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColorForMain(this.ClientRectangle, e);
        }
    }
}
