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
using Guna.UI2.WinForms;

namespace StepUp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            // Ensure that the event handlers are attached
            Text_username.TextChanged += Text_username_TextChanged;
            Text_password.TextChanged += Text_password_TextChanged;
        }

        public static class UserSession
        {
            public static int CustomerID { get; set; }
            public static string UserName { get; set; }
            public static int AdminID { get; set; } // Add this line
        }
        private void label2_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();
            
        }


        private void Button_Signin_Click(object sender, EventArgs e)
        {
            string email = Text_username.Text.Trim();
            string password = Text_password.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in both fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

            // Check if it's an admin login
            if (IsAdminLogin())
            {
                if (VerifyAdminLogin(email, password, connectionString))
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Navigate to the Admin Panel
                    AdminPanel adminPanel = new AdminPanel();
                    adminPanel.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else // It's a customer login
            {
                if (VerifyCustomerLogin(email, password, connectionString))
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Navigate to the main application form
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool IsAdminLogin()
        {
            // Assume you have a CheckBox named checkBoxAdmin for admin login
            return checkBoxAdmin.Checked;
        }

        private bool VerifyCustomerLogin(string email, string password, string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT CustomerID, Name FROM CustomerInfo WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); // Directly using the password

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // If a user is found, set the session details
                            UserSession.CustomerID = reader.GetInt32(0); // CustomerID
                            UserSession.UserName = reader.GetString(1); // Name
                            return true; // Login successful
                        }
                    }
                }
            }
            return false; // No match found
        }

        private bool VerifyAdminLogin(string email, string password, string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT AdminID, Name FROM AdminInfo WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); // Directly using the password

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // If an admin is found, set the session details
                            UserSession.AdminID = reader.GetInt32(0); // AdminID
                            UserSession.UserName = reader.GetString(1); // Name
                            return true; // Login successful
                        }
                    }
                }
            }
            return false; // No match found
        }

        private void UpdateTextBoxBorders()
        {
            // Set the border color to red for empty textboxes
            Text_username.BorderColor = string.IsNullOrWhiteSpace(Text_username.Text) ? Color.Red : Color.Silver;
            Text_password.BorderColor = string.IsNullOrWhiteSpace(Text_password.Text) ? Color.Red : Color.Silver;
        }

        

        private void Text_username_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBoxBorders();
        }
        private void Text_password_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBoxBorders();
        }

        private void Show_pass_CheckedChanged(object sender, EventArgs e)
        {
            Text_password.PasswordChar = Show_pass.Checked ? '\0' : '●';
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        
    }
}

