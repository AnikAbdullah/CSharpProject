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
    public partial class AddProductForm : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

        public AddProductForm()
        {
            InitializeComponent();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal productPrice;
            string productColor = txtProductColor.Text;
            int availableQuantity;
            int tag;

            if (decimal.TryParse(txtProductPrice.Text, out productPrice) &&
                int.TryParse(txtAvailableQuantity.Text, out availableQuantity) &&
                int.TryParse(txtTag.Text, out tag))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Products (ProductName, ProductPrice, ProductColor, AvailableQuantity, Tag) VALUES (@ProductName, @ProductPrice, @ProductColor, @AvailableQuantity, @Tag)", conn);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@ProductPrice", productPrice);
                    cmd.Parameters.AddWithValue("@ProductColor", productColor);
                    cmd.Parameters.AddWithValue("@AvailableQuantity", availableQuantity);
                    cmd.Parameters.AddWithValue("@Tag", tag);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product added successfully!");
                this.DialogResult = DialogResult.OK; // Indicate success
                this.Close(); // Close the form
            }
            else
            {
                MessageBox.Show("Please enter valid product details.");
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            this.Hide();
            adminPanel.Show();
        }

        private void guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            this.Hide();
            adminPanel.Show();
        }
    }
}
