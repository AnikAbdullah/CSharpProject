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

namespace StepUp
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Login l1 = new Login();
            l1.Show();
            this.Hide();
        }

        private void Button_SignUp_Click(object sender, EventArgs e)
        {
            string Email1 = Text_email.Text;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

            string tableName = CheckBox_Signup.Checked ? "AdminInfo" : "CustomerInfo";

            if (AreAllFieldsFilled())
            {
                // Email validation to check for the '@' symbol
                if (!IsValidEmail(Email1))
                {
                    MessageBox.Show("Please enter a valid email!", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (IsEmailExists(Email1, tableName, connectionString))
                    {
                        MessageBox.Show("Email already exists. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        string query = $"INSERT INTO {tableName} (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@Name", Text_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", Email1.Trim());
                            cmd.Parameters.AddWithValue("@Password", Text_password.Text.Trim());

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Account Created Successfully!", "Registered Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 f2 = new Form1();
                    f2.Show();
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while accessing the database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                UpdateTextBoxBorders();
            }
        }

        private bool IsValidEmail(string email)
        {
            // Basic validation to check if the email contains an '@' symbol
            return email.Contains("@") && (email.EndsWith(".com") || email.EndsWith(".edu"));
        }


        private bool AreAllFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(Text_name.Text) &&
                   !string.IsNullOrWhiteSpace(Text_email.Text) &&
                   !string.IsNullOrWhiteSpace(Text_password.Text);
        }

        private bool IsEmailExists(string email, string tableName, string connectionString)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    string query = $"SELECT COUNT(*) FROM {tableName} WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        return result != null && (int)result > 0; // Check for null before casting
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"An error occurred while checking the email: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Return false to avoid creating a new account if there was an error
            }
        }

        private void UpdateTextBoxBorders()
        {
            // Set the border color to red for empty textboxes
            Text_name.BorderColor = string.IsNullOrWhiteSpace(Text_name.Text) ? Color.Red : Color.Silver;
            Text_email.BorderColor = string.IsNullOrWhiteSpace(Text_email.Text) ? Color.Red : Color.Silver;
            Text_password.BorderColor = string.IsNullOrWhiteSpace(Text_password.Text) ? Color.Red : Color.Silver;
        }

        private void Show_pass_CheckedChanged(object sender, EventArgs e)
        {
            if (Text_password.PasswordChar == '●')
            {
                Text_password.PasswordChar = '\0';
            }
            else
            {
                Text_password.PasswordChar = '●';
            }
        }

        private void Text_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
