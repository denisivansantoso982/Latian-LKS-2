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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dtCustomer, dtItem, dtStatus, dtFD, dtRoomNumber;
        SqlCommand cmd;
        SqlDataReader reader;
        int id, priceItem, compensation, priceFD, total;

        public CheckOut()
        {
            InitializeComponent();
            loadDataRoom();
            loadDataStatus();
            loadDataItem();
            loadGridItem();
            loadGridFD();
            countTotalPrice();
            this.BackColor = ColourModel.primary;
        }

        void loadDataRoom()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select ReservationID, RoomNumber from ReservationRoom inner join Room on ReservationRoom.RoomID = Room.ID where GETDATE() > CheckInDateTime", connection);
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int price = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(2);
            textBox2.Text = (price * numericUpDown1.Value).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool same = false;
            int idItem = Convert.ToInt32(comboBoxItem.SelectedValue);
            int idStatus = Convert.ToInt32(comboBoxStatus.SelectedValue);

            foreach(DataGridViewRow row in gridItem.Rows )
            {
                if (gridItem.RowCount < 1 )
                {
                    gridItem.Rows.Add(idItem, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox2.Text), idStatus, comboBoxStatus.Text);
                } else
                {    
                    if ( row.Cells[0].Value.ToString().Equals(idItem.ToString()) && row.Cells[5].Value.ToString().Equals(idStatus.ToString()) )
                    {
                        same = true;
                        break;
                    } else
                    {
                        same = false;
                    }
                }
            }

            if ( same )
            {
                gridItem.Rows.Remove(gridItem.SelectedRows[0]);
                gridItem.Rows.Add(idItem, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox2.Text), idStatus, comboBoxStatus.Text);
            } else
            {
                gridItem.Rows.Add(idItem, comboBoxItem.Text, numericUpDown1.Value, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox2.Text), idStatus, comboBoxStatus.Text);
            }

            countTotalPrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gridItem.Rows.Remove(gridItem.SelectedRows[0]);

            countTotalPrice();
        }

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int price = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(2);
            int compensationFee = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(3);
            textBox2.Text = (price * numericUpDown1.Value).ToString();
            textBox3.Text = compensationFee.ToString();
        }

        private void gridFD_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string val = e.Value.ToString();

            switch ( val )
            {
                case "F":
                    e.Value = "Food";
                    break;
                case "D":
                    e.Value = "Drink";
                    break;
            }
        }

        void loadDataStatus()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select * from ItemStatus", connection);
                dtStatus = new DataTable();
                adapter.Fill(dtStatus);

                comboBoxStatus.DataSource = dtStatus;
                comboBoxStatus.DisplayMember = "Name";
                comboBoxStatus.ValueMember = "ID";

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
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
                textBox2.Text = (price * numericUpDown1.Value).ToString();
                textBox3.Text = dtItem.Rows[comboBoxItem.SelectedIndex].Field<int>(3).ToString();

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadGridItem()
        {
            gridItem.Columns.Add("IDItem", "ID");
            gridItem.Columns.Add("ItemName", "Name");
            gridItem.Columns.Add("Qty", "Quantity");
            gridItem.Columns.Add("Compensation", "Compensation Fee");
            gridItem.Columns.Add("SubTotal", "Sub Total");
            gridItem.Columns.Add("IDStatus", "IDStatus");
            gridItem.Columns.Add("Status", "Status");

            gridItem.Columns[0].Visible = false;
            gridItem.Columns[5].Visible = false;

            gridItem.Columns[3].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
            gridItem.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridItem.Columns[4].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
            gridItem.Columns[4].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void loadGridFD()
        {
            try
            {
                connection.Open();

                adapter = new SqlDataAdapter("select FoodsAndDrinks.Name, Price, FDCheckOut.*, Type from FDCheckOut inner join FoodsAndDrinks on FDCheckOut.FDID = FoodsAndDrinks.ID where ReservationRoomID = " + comboBoxNumber.SelectedValue, connection);
                dtFD = new DataTable();
                adapter.Fill(dtFD);

                gridFD.DataSource = dtFD;
                gridFD.Columns[2].Visible = false;
                gridFD.Columns[3].Visible = false;
                gridFD.Columns[4].Visible = false;
                gridFD.Columns[7].Visible = false;

                gridFD.Columns[4].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
                gridFD.Columns[4].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridFD.Columns[6].CellTemplate.Style.Font = new Font(gridItem.Font, FontStyle.Bold);
                gridFD.Columns[6].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                connection.Close();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckOut_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void countTotalPrice()
        {
            priceItem = 0;
            compensation = 0;
            priceFD = 0;
            total = 0;

            foreach (DataGridViewRow row in gridItem.Rows )
            {
                int idStatus = Convert.ToInt32(row.Cells[5].Value);
                compensation = Convert.ToInt32(row.Cells[4].Value);
                int subTotal = Convert.ToInt32(row.Cells[3].Value);

                if (idStatus == 1 || idStatus == 2 )
                {
                    priceItem = priceItem + compensation + subTotal;
                } else
                {
                    priceItem = priceItem + subTotal;
                }
            }

            foreach ( DataGridViewRow row in gridFD.Rows )
            {
                int price = Convert.ToInt32(row.Cells[1].Value);
                int qty = Convert.ToInt32(row.Cells[5].Value);
                priceFD = priceFD + (price * qty);
            }

            total = priceFD + priceItem;

            labelFD.Text = string.Concat("Rp. ", priceFD.ToString());
            labelItem.Text = string.Concat("Rp. ", priceItem.ToString());
            labelTotalPrice.Text = string.Concat("Rp. ", total.ToString());
        }
    }
}
