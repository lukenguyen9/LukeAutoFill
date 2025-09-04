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
using Tesseract;

namespace LukeAutoFill
{
    public partial class Autofill : Form
    {
        private string name;
        private string doctor;
        private string medicine;

        public Autofill()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxImage.Image = Image.FromFile(ofd.FileName);
                    pictureBoxImage.Tag = ofd.FileName; // Save file path for OCR
                }
            }
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {
        string imagePath = pictureBoxImage.Tag?.ToString();
        if (string.IsNullOrEmpty(imagePath))
        {
            MessageBox.Show("Please upload an image first.");
            return;
        }

        try
        {
            using (var ocrEngine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            using (var img = Pix.LoadFromFile(imagePath))
            using (var page = ocrEngine.Process(img))
            {
                string rawText = page.GetText();
                richTextBox1.Text = rawText; // display in optional box

                AutoFillFields(rawText); // fill fields
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("OCR failed: " + ex.Message);
        }
    }
        private void AutoFillFields(string text)
        {
            // 1. Reset fields every time
            textBoxName.Text = "no value";
            textBoxMedicine.Text = "no value";
            textBoxInstruction.Text = "no value";
            textBoxDoctor.Text = "no value";

            name = string.Empty;
            medicine = string.Empty;
            doctor = string.Empty;

            // 2. Extract values or leave as "no value"

            if (text.Contains("Patient:"))
            {
                int start = text.IndexOf("Patient:") + 8;
                int end = text.IndexOf("\n", start);
                if (end == -1) end = text.Length;

                name = text.Substring(start, end - start).Trim();
                if (!string.IsNullOrEmpty(name))
                    textBoxName.Text = name;
            }

            if (text.Contains("Medicine:"))
            {
                int start = text.IndexOf("Medicine:") + 9;
                int end = text.IndexOf("\n", start);
                if (end == -1) end = text.Length;

                medicine = text.Substring(start, end - start).Trim();
                if (!string.IsNullOrEmpty(medicine))
                    textBoxMedicine.Text = medicine;
            }

            if (text.Contains("Takes"))
            {
                int start = text.IndexOf("Takes") + "Takes".Length;

                // Skip whitespace after "Takes"
                while (start < text.Length && char.IsWhiteSpace(text[start]))
                    start++;

                int end = text.IndexOf('\n', start);
                if (end == -1) end = text.Length;

                string result = text.Substring(start, end - start).Trim();
                if (!string.IsNullOrEmpty(result))
                    textBoxInstruction.Text = result;
            }

            if (text.Contains("Doctor:"))
            {
                int start = text.IndexOf("Doctor:") + 7;
                int end = text.IndexOf("\n", start);
                if (end == -1) end = text.Length;

                doctor = text.Substring(start, end - start).Trim();
                if (!string.IsNullOrEmpty(doctor))
                    textBoxDoctor.Text = doctor;
            }
        }
        

        private void SaveToDatabase(object sender, EventArgs e)
        {
            string connectionString = "Server=acarepharmacy00\\sqlexpress;Database=LukeLoginTest;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO testingpatient (Patient, Doctor, medicine) VALUES (@Patient, @Doctor, @medicine)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Patient", name);
                    cmd.Parameters.AddWithValue("@Doctor", doctor);
                    cmd.Parameters.AddWithValue("@medicine", medicine);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBox.Show("Record saved to 'testingpatient'.");
        }
    }
}
