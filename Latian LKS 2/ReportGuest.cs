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
using System.Windows.Forms.DataVisualization.Charting;

namespace Latian_LKS_2
{
    public partial class ReportGuest : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dtToday, dtRange;
        SqlCommand cmd;
        SqlDataReader reader;

        public ReportGuest()
        {
            InitializeComponent();
            loadComboBoxSelecTime();
            loadDataChart();
            this.BackColor = ColourModel.primary;
        }

        void loadComboBoxSelecTime()
        {
            string[] time = { "Today", "Pick Date Range" };
            comboBoxDate.DataSource = time;
        }

        void loadDataChart()
        {
            try
            {
                connection.Open();

                string query = "select Reservation.ID, DateTime, BookingCode, Customer.Name, RoomType.ID, RoomNumber, RoomType.RoomPrice, CheckInDateTime, CheckOutDateTime from Reservation inner join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID inner join Customer on Reservation.CustomerID = Customer.ID inner join Room on ReservationRoom.RoomID = Room.ID inner join RoomType on Room.RoomTypeID = RoomType.ID";
                string queryChart = "select distinct(DateTime) as date, COUNT(DateTime) as count from Reservation";

                if (comboBoxDate.SelectedIndex == 0 )
                {
                    cmd = new SqlCommand(queryChart + " where DateTime = CONVERT(date, @date) group by DateTime", connection);
                    cmd.Parameters.AddWithValue("@date", SqlDbType.Date).Value = datePickerFrom.Value.Date;
                    reader = cmd.ExecuteReader();
                    Series series = new Series();
                    while ( reader.Read() )
                    {
                        string date = reader.GetDateTime(0).ToString("dd MMMM yyyy");
                        chartReport.Series[0].Points.AddXY(date, reader.GetInt32(1));
                    }
                    reader.Close();
                } else
                {
                    cmd = new SqlCommand(queryChart + " where DateTime between @date1 and @date2 group by DateTime", connection);
                    cmd.Parameters.AddWithValue("@date1", SqlDbType.Date).Value = datePickerFrom.Value.Date;
                    cmd.Parameters.AddWithValue("@date2", SqlDbType.Date).Value = datePickerTo.Value.Date;
                    reader = cmd.ExecuteReader();
                    Series series = new Series();
                    chartReport.Series[0].Points.Clear();
                    while ( reader.Read() )
                    {
                        string date = reader.GetDateTime(0).ToString("dd MMMM yyyy hh:mm:ss");
                        chartReport.Series[0].Points.AddXY(date, reader.GetInt32(1));
                    }
                    reader.Close();
                }

                connection.Close();
            } catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReportGuest_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            loadDataChart();
        }

        private void comboBoxDate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxDate.SelectedIndex == 0 )
            {
                label1.Visible = false;
                label6.Visible = false;
                datePickerFrom.Visible = false;
                datePickerTo.Visible = false;
            } else if (comboBoxDate.SelectedIndex == 1 )
            {
                label1.Visible = true;
                label6.Visible = true;
                datePickerFrom.Visible = true;
                datePickerTo.Visible = true;
            }

            loadDataChart();
        }
    }
}
