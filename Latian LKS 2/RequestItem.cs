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
    public partial class RequestItem : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dtRoomNumber, dtItem;
        SqlCommand cmd;
        bool sameItem;

        public RequestItem()
        {
            InitializeComponent();
            loadDataRoom();
            loadDataItem();
            loadGridItem();
            countTotalPrice();
            this.BackColor = ColourModel.primary;
        }

        void loadDataRoom()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select ReservationID, RoomNumber from ReservationRoom inner join Room on ReservationRoom.RoomID = Room.ID", connection);
                dtRoomNumber = new DataTable();
                adapter.Fill(dtRoomNumber);

                comboBoxNumber.DataSource = dtRoomNumber;
                comboBoxNumber.DisplayMember = "RoomNumber";
                comboBoxNumber.ValueMember = "ReservationID";

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadGridItem()
        {
            gridItem.Columns.Add("ID", "ID");
            gridItem.Columns.Add("Name", "Name");
            gridItem.Columns.Add("Qty", "Quantity");
            gridItem.Columns.Add("RequestPrice", "Price");
            gridItem.Columns.Add("CompensationFee", "Compensation");
            gridItem.Columns.Add("SubTotal", "Sub Total");

            gridItem.Columns[0].Visible = false;
            gridItem.Columns[4].Visible = false;

            gridItem.Columns[3].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
            gridItem.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridItem.Columns[5].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
            gridItem.Columns[5].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void loadDataItem()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT * FROM Item", connection);
                dtItem = new DataTable();
                adapter.Fill(dtItem);

                comboBoxItem.DataSource = dtItem;
                comboBoxItem.DisplayMember = "Name";
                comboBoxItem.ValueMember = "ID";
                int price = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(2);
                textBox1.Text = price.ToString();
                textBox2.Text = (price * numericUpDown1.Value).ToString();

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RequestItem_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColor(this.ClientRectangle, e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int subTotal = Convert.ToInt32(textBox1.Text) * Convert.ToInt32(numericUpDown1.Value);
            textBox2.Text = subTotal.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sameItem = true;
            int compensation = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(3);
            if ( gridItem.RowCount == 0 )
            {
                gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
            }
            else
            {
                foreach ( DataGridViewRow row in gridItem.Rows )
                {
                    int id = Convert.ToInt32(comboBoxItem.SelectedValue);
                    if ( id == Convert.ToInt32(row.Cells[0].Value) )
                    {
                        sameItem = true;
                        gridItem.Rows.Remove(row);
                    }
                    else
                    {
                        sameItem = false;
                    }
                }

                if ( !sameItem )
                    gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
                else
                    gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
            }

            countTotalPrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach ( DataGridViewRow row in gridItem.SelectedRows )
                gridItem.Rows.Remove(row);

            countTotalPrice();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                for (int i = 1; i <= gridItem.RowCount; i++ )
                {
                    cmd = new SqlCommand("INSERT INTO ReservationRequestItem VALUES(" + comboBoxNumber.SelectedValue + ", " + comboBoxItem.SelectedValue + ", " + Convert.ToInt32(gridItem.Rows[i - 1].Cells[2].Value) + ", " + Convert.ToInt32(gridItem.Rows[i - 1].Cells[5].Value) + ")", connection);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Request item added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                connection.Close();
            } catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int price = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(2);
            textBox1.Text = price.ToString();
            textBox2.Text = (price * numericUpDown1.Value).ToString();
        }

        void countTotalPrice()
        {
            int price = 0;

            foreach ( DataGridViewRow row in gridItem.Rows )
                price += Convert.ToInt32(row.Cells[5].Value);

            labelTotalPrice.Text = string.Concat("Rp. ", price.ToString());
        }
    }
}
