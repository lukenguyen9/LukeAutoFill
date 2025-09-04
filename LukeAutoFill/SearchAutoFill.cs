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
            string query = "SELECT Patient, Doctor, medicine FROM testingpatient WHERE Patient LIKE @name";

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
            }
        }

        private void backButton(object sender, EventArgs e)
        {
            Autofill fillForm = new Autofill();
            fillForm.Show(); // or ShowDialog() if you want it modal
        }
    }
}
