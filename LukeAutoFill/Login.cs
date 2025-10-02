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

namespace LukeAutoFill
{
    public partial class Login : Form
    {
        public User user = null;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Please enter email and password",
                    "Email or Password Empty",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            try
            {
                string connString = "Server=acarepharmacy00\\sqlexpress;Database=LukeLoginTest;Trusted_Connection=True;";
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    sqlConn.Open();

                    string sqlQuery = "SELECT * FROM users WHERE email=@email AND password=@pwd";
                    using (SqlCommand command = new SqlCommand(sqlQuery, sqlConn))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@pwd", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3)
                                };

                                Autofill autofillForm = new Autofill();
                                autofillForm.Show();
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Email or password is incorrect",
                                    "Email or Password Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to connect to database: " + ex.Message,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSignUp(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            signUpForm.Show();

        }
    }
}
