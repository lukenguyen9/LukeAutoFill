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

namespace LukeAutoFill
{
    public partial class ProfitChart : Form
    {
        public ProfitChart()
        {
            InitializeComponent();
        }

        private void btnProfitDay(object sender, EventArgs e)
        {
            string connString = "Server=acarepharmacy00\\sqlexpress;Database=LukeLoginTest;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = @"
                    SELECT CAST(CreatedAt AS DATE) AS SaleDate, SUM(Price) AS TotalProfit
                    FROM testingpatient
                    GROUP BY CAST(CreatedAt AS DATE)
                    ORDER BY SaleDate;
                ";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Clear any old series
                chart1.Series.Clear();

                // Setup chart area
                chart1.ChartAreas[0].AxisX.Title = "Date";
                chart1.ChartAreas[0].AxisY.Title = "Profit ($)";
                chart1.ChartAreas[0].AxisX.Interval = 1; // show every date

                // Add new series
                Series series = new Series("Profit");
                series.ChartType = SeriesChartType.Bar; // Line, Bar, or Column
                series.XValueType = ChartValueType.Date;

                foreach (DataRow row in dt.Rows)
                {
                    DateTime saleDate = Convert.ToDateTime(row["SaleDate"]);
                    decimal profit = Convert.ToDecimal(row["TotalProfit"]);
                    series.Points.AddXY(saleDate, profit);
                }

                chart1.Series.Add(series);
            }
        }

        private void totalProfitBtn_Click(object sender, EventArgs e)
        {
            string connString = "Server=acarepharmacy00\\sqlexpress;Database=LukeLoginTest;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = @"
            SELECT CAST(CreatedAt AS DATE) AS SaleDate,
                   SUM(SUM(Price)) OVER (ORDER BY CAST(CreatedAt AS DATE)) AS CumulativeProfit
            FROM testingpatient
            GROUP BY CAST(CreatedAt AS DATE)
            ORDER BY SaleDate;
        ";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.Series.Clear();

                Series series = new Series("Cumulative Profit");
                series.ChartType = SeriesChartType.Line; // line chart is best for growth
                series.XValueType = ChartValueType.Date;

                foreach (DataRow row in dt.Rows)
                {
                    DateTime saleDate = Convert.ToDateTime(row["SaleDate"]);
                    decimal profit = Convert.ToDecimal(row["CumulativeProfit"]);
                    series.Points.AddXY(saleDate, profit);
                }

                chart1.Series.Add(series);

                chart1.ChartAreas[0].AxisX.Title = "Date";
                chart1.ChartAreas[0].AxisY.Title = "Cumulative Profit ($)";

                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autofill autofillForm = new Autofill();
            autofillForm.Show();
            this.Close();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAutoFill searchForm = new SearchAutoFill();
            searchForm.Show();
            this.Close();
        }

        private void signUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            signUpForm.Show();
            this.Close();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }
    }
}
