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
using Excel = Microsoft.Office.Interop.Excel;

namespace Latian_LKS_2
{
    public partial class ReportGuest : Form
    {
        SqlConnection connection = new SqlConnection(Connection.connectionString);
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        SqlDataReader reader;

        public ReportGuest()
        {
            InitializeComponent();
            loadComboBoxSelecTime();
            loadDataChart();
            loadGrid();
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

        void loadGrid()
        {
            connection.Open();

            if (comboBoxDate.SelectedIndex == 0)
            {
                adapter = new SqlDataAdapter("select Reservation.ID, DateTime, Employee.Name, Customer.Name, BookingCode from Reservation inner join Customer on Reservation.CustomerID = Customer.ID inner join Employee on Reservation.EmployeeID = Employee.ID where DateTime = '" + DateTime.Now.Date + "'", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridGuest.DataSource = dt;
                gridGuest.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                gridGuest.Columns[1].HeaderText = "Date Time";
                gridGuest.Columns[2].HeaderText = "Employee";
                gridGuest.Columns[3].HeaderText = "Customer";
                gridGuest.Columns[4].HeaderText = "Booking Code";
            } else
            {
                adapter = new SqlDataAdapter("select Reservation.ID, DateTime, Employee.Name, Customer.Name, BookingCode from Reservation inner join Customer on Reservation.CustomerID = Customer.ID inner join Employee on Reservation.EmployeeID = Employee.ID where DateTime between '" + datePickerFrom.Value.Date + "' and '" + datePickerTo.Value.Date + "'", connection);
                dt = new DataTable();
                adapter.Fill(dt);

                gridGuest.DataSource = dt;
                gridGuest.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                gridGuest.Columns[1].HeaderText = "Date Time";
                gridGuest.Columns[2].HeaderText = "Employee";
                gridGuest.Columns[3].HeaderText = "Customer";
                gridGuest.Columns[4].HeaderText = "Booking Code";
            }

            connection.Close();
        }

        private void ReportGuest_Paint(object sender, PaintEventArgs e)
        {
            GradientModel.gradientColor(this.ClientRectangle, e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            loadDataChart();
            loadGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = null;

            app.Visible = true;

            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report Guest";

            for (int i = 1; i < gridGuest.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = gridGuest.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < gridGuest.Rows.Count - 1; i++)
            {
                for (int j = 0; j < gridGuest.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = gridGuest.Rows[i].Cells[j].Value.ToString();
                }
            }

            worksheet.SaveAs("D:\\DENIS IVAN SANTOSO\\Project Example\\Desktop_Tutorial\\LKS\\Latian LKS 2\\Guest.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing);
            workbook.Close(true, Type.Missing, Type.Missing);
            app.Quit();
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
            loadGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gridGuest.ClearSelection();
            printPreviewDialog.Document = printDocument;
            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void gridGuest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Visible = true;
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int center = printDocument.DefaultPageSettings.PaperSize.Width / 2;
            int left = printDocument.DefaultPageSettings.Margins.Left;
            int right = printDocument.DefaultPageSettings.PaperSize.Width;

            StringFormat centerText = new StringFormat();
            StringFormat leftText = new StringFormat();
            StringFormat rightText = new StringFormat();

            centerText.Alignment = StringAlignment.Center;
            leftText.Alignment = StringAlignment.Near;
            rightText.Alignment = StringAlignment.Far;

            Font headerFont = new Font("Nirmala UI", 12, FontStyle.Bold);
            Font contentFont = new Font("Nirmala UI", 11, FontStyle.Regular);

            e.Graphics.DrawString("JAVA HOTEL", new Font("Nirmala UI", 20, FontStyle.Bold), Brushes.Black, center, 20, centerText);

            Bitmap bmp = new Bitmap(gridGuest.Width, gridGuest.Height);

            int height = 100;
            int horizontalMargin = 0;
            for (int i = 1; i < gridGuest.Columns.Count; i++)
            {
                e.Graphics.DrawString(gridGuest.Columns[i].HeaderText, headerFont, Brushes.Black, 20 + horizontalMargin, height);
                horizontalMargin += printDocument.DefaultPageSettings.PaperSize.Width / 4;
            }

            height += 10;

            foreach (DataGridViewRow row in gridGuest.Rows)
            {
                string text = row.ToString();
                height += 30;
                horizontalMargin = 0;
                for (int i = 1; i < gridGuest.Columns.Count; i++)
                {
                    e.Graphics.DrawString(row.Cells[i].Value.ToString(), contentFont, Brushes.Black, 20 + horizontalMargin, height);
                    horizontalMargin += printDocument.DefaultPageSettings.PaperSize.Width / 4;
                }
            }
        }
    }
}
