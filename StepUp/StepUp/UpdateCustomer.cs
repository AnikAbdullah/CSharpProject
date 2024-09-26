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

namespace StepUp
{
    public partial class UpdateCustomer : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";
        private int customerId;
        public UpdateCustomer(int id)
        {
            InitializeComponent();
            this.customerId = id;
            LoadCustomerData();
        }
        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CustomerInfo WHERE CustomerID = @CustomerId", conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Assuming you have TextBoxes named txtName, txtEmail, and txtPassword
                    txtName.Text = reader["Name"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtPassword.Text = reader["Password"].ToString();
                }
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            // Update customer information based on the fields entered
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE CustomerInfo SET Name = @Name, Email = @Email, Password = @Password WHERE CustomerID = @CustomerId", conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Customer updated successfully.");
                    this.Close(); // Close the form after update
                }
                else
                {
                    MessageBox.Show("Failed to update the customer.");
                }
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            this.Hide();
            adminPanel.Show();
        }
    }
}
