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
    public partial class Item : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        int id;
        public Item()
        {
            InitializeComponent();
            hide();
            loadDataItem();
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
            id = 0;

            button3.Visible = false;
            button4.Visible = false;
        }

        void loadDataItem()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM Item", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridItem.DataSource = dt;
                gridItem.Columns[0].Visible = false;

                gridItem.Columns[2].HeaderText = "Request Price";
                gridItem.Columns[3].HeaderText = "Compensation Fee";

                gridItem.Columns[2].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridItem.Columns[2].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
                gridItem.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridItem.Columns[3].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);

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

        private void Item_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColor(this.ClientRectangle, e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
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

                    cmd = new SqlCommand("INSERT INTO Item VALUES('" + textBox1.Text + "', " + Convert.ToInt32(textBox2.Text) + ", " + Convert.ToInt32(textBox3.Text) + ")", connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data item added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataItem();
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
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ( validation() )
                {
                    connection.Open();

                    cmd = new SqlCommand("UPDATE Item SET Name = '" + textBox1.Text + "', RequestPrice = " + Convert.ToInt32(textBox2.Text) + ", CompensationFee = " + Convert.ToInt32(textBox3.Text) + " WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Item updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataItem();
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

                    cmd = new SqlCommand("DELETE FROM Item WHERE ID = " + id, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data item deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    loadDataItem();
                    hide();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = gridItem.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = gridItem.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = gridItem.SelectedRows[0].Cells[3].Value.ToString();
            id = (int) gridItem.SelectedRows[0].Cells[0].Value;

            show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hide();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            loadDataItem();
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBox5.Text);
            gridItem.DataSource = dv;
        }
    }
}
