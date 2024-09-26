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
    public partial class AddCustomerForm : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";
        public AddCustomerForm()
        {
            InitializeComponent();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO CustomerInfo (Name, Email, Password) VALUES (@Name, @Email, @Password)", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Customer added successfully!");
            this.DialogResult = DialogResult.OK; // Indicate success
            this.Close(); // Close the form
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            this.Hide();
            adminPanel.Show();
        }

    }
}
