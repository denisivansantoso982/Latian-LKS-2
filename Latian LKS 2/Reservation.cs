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
    public partial class Reservation : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dtCustomer, dtItem, dtAvailable, dtRoomType;
        SqlCommand cmd;
        SqlDataReader reader;
        int totalPrice, priceRoom, priceItem;
        bool sameRoom, sameItem;

        public Reservation()
        {
            InitializeComponent();
            loadDataRoomType();
            loadDataItem();
            loadDataCustomer();
            loadDataRoomAvailable();
            loadDataRoomSelected();
            loadGridItem();
            countTotalPrice();
            dateTimePicker.MinDate = DateTime.Now;
            dateTimePicker1.MinDate = dateTimePicker.Value.AddDays(1);
            this.BackColor = ColourModel.primary;
        }

        void loadDataCustomer()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select * from Customer", connection);
                dtCustomer = new DataTable();
                adapter.Fill(dtCustomer);

                gridCustomer.DataSource = dtCustomer;
                gridCustomer.Columns[0].Visible = false;
                gridCustomer.Columns[2].Visible = false;
                gridCustomer.Columns[5].Visible = false;
                gridCustomer.Columns[6].Visible = false;

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

            gridItem.Columns[3].CellTemplate.Style.Font = new Font(gridAvailable.Font, FontStyle.Bold);
            gridItem.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridItem.Columns[5].CellTemplate.Style.Font = new Font(gridAvailable.Font, FontStyle.Bold);
            gridItem.Columns[5].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void loadDataRoomAvailable()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select Room.ID, Name, RoomNumber, RoomFloor, Description, RoomPrice, RoomType.ID from Room inner join RoomType on Room.RoomTypeID = RoomType.ID WHERE RoomTypeID = " + comboBoxType.SelectedValue, connection);
                dtAvailable = new DataTable();
                adapter.Fill(dtAvailable);

                gridAvailable.DataSource = dtAvailable;
                gridAvailable.Columns[0].Visible = false;
                gridAvailable.Columns[4].Visible = false;
                gridAvailable.Columns[6].Visible = false;

                gridAvailable.Columns[2].HeaderText = "Number";
                gridAvailable.Columns[3].HeaderText = "Floor";
                gridAvailable.Columns[5].HeaderText = "Price";

                gridAvailable.Columns[5].CellTemplate.Style.Font = new Font(gridAvailable.Font, FontStyle.Bold);
                gridAvailable.Columns[5].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDataRoomSelected()
        {
            gridSelected.Columns.Add("ID", "ID");
            gridSelected.Columns.Add("Name", "Name");
            gridSelected.Columns.Add("Number", "Number");
            gridSelected.Columns.Add("Floor", "Floor");
            gridSelected.Columns.Add("Description", "Description");
            gridSelected.Columns.Add("Price", "Price");
            gridSelected.Columns.Add("RoomTypeID", "RoomTypeID");

            gridSelected.Columns[0].Visible = false;
            gridSelected.Columns[4].Visible = false;
            gridSelected.Columns[6].Visible = false;

            gridSelected.Columns[5].CellTemplate.Style.Font = new Font(gridSelected.Font, FontStyle.Bold);
            gridSelected.Columns[5].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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

        void loadDataRoomType()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("SELECT ID, Name FROM RoomType", connection);
                dtRoomType = new DataTable();
                adapter.Fill(dtRoomType);

                comboBoxType.DataSource = dtRoomType;
                comboBoxType.DisplayMember = "Name";
                comboBoxType.ValueMember = "ID";

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

        private void gridCustomer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string val = e.Value.ToString();

            switch ( val )
            {
                case "M":
                    e.Value = "Male";
                    break;
                case "F":
                    e.Value = "Female";
                    break;
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            loadDataCustomer();
            DataView dv = new DataView(dtCustomer);
            dv.RowFilter = String.Format("Name LIKE '%{0}%'", textBoxSearch.Text);
            gridCustomer.DataSource = dv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sameRoom = false;

            foreach (DataGridViewRow row in gridAvailable.SelectedRows )
            {
                int idRoom = (int) row.Cells[0].Value;

                if (gridSelected.RowCount < 1 )
                {
                    gridSelected.Rows.Add(row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value, row.Cells[5].Value, row.Cells[6].Value);
                    break;
                } else
                {
                    foreach (DataGridViewRow rowValue in gridSelected.Rows )
                    {
                        if ( row.Cells[0].Value.ToString().Equals(rowValue.Cells[0].Value.ToString()) )
                        {
                            sameRoom = true;
                            break;
                        } else
                        {
                            sameRoom = false;
                        }
                    }

                    if (!sameRoom)
                        gridSelected.Rows.Add(row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value, row.Cells[5].Value, row.Cells[6].Value);
                    else
                        MessageBox.Show("Can not add the same Room!");
                }
            }

            countTotalPrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach ( DataGridViewRow row in gridSelected.SelectedRows )
            {
                if (gridSelected.RowCount >= 1)
                    gridSelected.Rows.Remove(row);
            }

            countTotalPrice();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sameItem = true;
            int compensation = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(3);
            if (gridItem.RowCount == 0 )
            {
                gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
            } else
            {
                foreach (DataGridViewRow row in gridItem.Rows )
                {
                    int id = Convert.ToInt32(comboBoxItem.SelectedValue);
                    if (id == Convert.ToInt32(row.Cells[0].Value) )
                    {
                        sameItem = true;
                        gridItem.Rows.Remove(row);
                    }
                    else
                    {
                        sameItem = false;
                    }
                }

                if (!sameItem)
                    gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
                else
                    gridItem.Rows.Add(comboBoxItem.SelectedValue, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox1.Text), compensation, Convert.ToInt32(textBox2.Text));
            }

            countTotalPrice();
        }

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int price = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(2);
            textBox1.Text = price.ToString();
            textBox2.Text = (price * numericUpDown1.Value).ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int subTotal = Convert.ToInt32(textBox1.Text) * Convert.ToInt32(numericUpDown1.Value);
            textBox2.Text = subTotal.ToString();
        }

        private void textBoxStay_TextChanged(object sender, EventArgs e)
        {
            countTotalPrice();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach ( DataGridViewRow row in gridItem.SelectedRows )
                gridItem.Rows.Remove(row);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxStay.Text == "" && gridSelected.RowCount < 1 )
                {
                    MessageBox.Show("Please Fill the form!");
                } else
                {
                    connection.Open();

                    int customerID = Convert.ToInt32(gridCustomer.SelectedRows[0].Cells[0].Value);
                    Guid guidString = Guid.NewGuid();
                    string random = Convert.ToBase64String(guidString.ToByteArray());
                    random = random.Replace("=", "");
                    random = random.Replace("\\", "");
                    random = random.Replace("/", "");
                    random = random.Replace("@", "");
                    random = random.Replace("$", "");
                    random = random.Replace("+", "");
                    random = random.Substring(0, 6);

                    cmd = new SqlCommand("INSERT INTO Reservation VALUES('" + DateTime.Now + "', " + EmployeeModel.ID + ", " + customerID + ", '" + random + "')", connection);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT ID FROM Reservation WHERE BookingCode = '" + random + "'", connection);
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if ( reader.HasRows )
                    {
                        int idReservation = Convert.ToInt32(reader["ID"]);
                        reader.Close();

                        for (int i = 1; i <= gridSelected.RowCount; i++ )
                        {
                            int idRoom = Convert.ToInt32(gridSelected.Rows[i - 1].Cells[0].Value);
                            cmd = new SqlCommand("INSERT INTO ReservationRoom VALUES(" + idReservation + ", " + idRoom + ", '" + dateTimePicker.Value + "', " + Convert.ToInt32(textBoxStay.Text) + ", " + Convert.ToInt32(gridSelected.Rows[i - 1].Cells[5].Value) + ", '" + dateTimePicker.Value + "', '" + dateTimePicker1.Value + "')", connection);
                            cmd.ExecuteNonQuery();
                        }

                        for ( int i = 1; i <= gridItem.RowCount; i++ )
                        {
                            cmd = new SqlCommand("INSERT INTO ReservationRequestItem VALUES(" + idReservation + ", " + Convert.ToInt32(gridItem.Rows[i - 1].Cells[0].Value) + ", " + Convert.ToInt32(gridItem.Rows[i - 1].Cells[2].Value) + ", " + Convert.ToInt32(gridItem.Rows[i - 1].Cells[5].Value) + ")", connection);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Data reservation added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                    connection.Close();
                }
            } catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddNewCustomer newCustomer = new AddNewCustomer();
            newCustomer.ShowDialog();
        }

        private void comboBoxType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            loadDataRoomAvailable();
        }

        private void Reservation_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void gridCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxStay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        void countTotalPrice()
        {
            totalPrice = 0;
            priceItem = 0;
            priceRoom = 0;

            for (int i = 1; i <= gridSelected.RowCount; i++ )
                priceRoom += Convert.ToInt32(gridSelected.Rows[i - 1].Cells[5].Value);


            for ( int i = 1; i <= gridItem.RowCount; i++ )
                priceItem += Convert.ToInt32(gridItem.Rows[i - 1].Cells[5].Value);

            int stay;
            if ( textBoxStay.Text == "" )
                stay = 1;
            else
                stay = Convert.ToInt32(textBoxStay.Text);

            totalPrice = priceItem + (priceRoom * stay);
            labelTotalPrice.Text = string.Concat("Rp. ", totalPrice.ToString());
        }
    }
}
