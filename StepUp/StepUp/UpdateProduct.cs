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
    public partial class UpdateProduct : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";
        private int productId;
        public UpdateProduct(int id)
        {
            InitializeComponent();
            productId = id;
            LoadProductData();
        }

        private void LoadProductData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtProductName.Text = reader["ProductName"].ToString();
                    txtProductColor.Text = reader["ProductColor"].ToString();
                    txtProductPrice.Text = reader["ProductPrice"].ToString();
                    txtAvailableQuantity.Text = reader["AvailableQuantity"].ToString();
                }
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            string productColor = txtProductColor.Text;
            decimal productPrice = decimal.Parse(txtProductPrice.Text);
            int availableQuantity = int.Parse(txtAvailableQuantity.Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Products SET ProductName = @ProductName, ProductColor = @ProductColor, ProductPrice = @ProductPrice, AvailableQuantity = @AvailableQuantity WHERE ProductID = @ProductID", conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@ProductColor", productColor);
                cmd.Parameters.AddWithValue("@ProductPrice", productPrice);
                cmd.Parameters.AddWithValue("@AvailableQuantity", availableQuantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product updated successfully!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
