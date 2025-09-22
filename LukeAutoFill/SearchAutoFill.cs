using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace LukeAutoFill
{
    public partial class SearchAutoFill : Form
    {
        public SearchAutoFill()
        {
            InitializeComponent();
        }


        private void searchBtn(object sender, EventArgs e)
        {
            string nameToSearch = textBox1.Text;

            string connectionString = "Data Source=acarepharmacy00\\sqlexpress;Initial Catalog=LukeLoginTest;Integrated Security=True;";
            string query = "SELECT Patient, Doctor, medicine, Price FROM testingpatient WHERE Patient LIKE @name";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", "%" + nameToSearch + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable results = new DataTable();

                    connection.Open();
                    adapter.Fill(results);

                    dataGridResults.DataSource = results;
                    CalculateTotalPrice(nameToSearch);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
            }
        }
        private void totalPrice_TextChanged(object sender, EventArgs e)
        {

        }
        private void CalculateTotalPrice(string patientName)
        {
            string connectionString = "Server=acarepharmacy00\\sqlexpress;Database=LukeLoginTest;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Sum all prices for the given patient
                    string query = "SELECT SUM(Price) FROM testingpatient WHERE Patient = @Patient";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Patient", patientName);

                        object result = cmd.ExecuteScalar(); // returns the sum
                        decimal total = 0m;

                        if (result != DBNull.Value)
                            total = Convert.ToDecimal(result);

                        totalPrice.Text = total.ToString("0.00"); // display formatted
                    }

                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void backButton(object sender, EventArgs e)
        {
            Autofill fillForm = new Autofill();
            fillForm.Show(); // or ShowDialog() if you want it modal
            this.Close();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autofill autofillForm = new Autofill();
            autofillForm.Show();
            this.Close();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        private void signUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            signUpForm.Show();
        }


    }
}
