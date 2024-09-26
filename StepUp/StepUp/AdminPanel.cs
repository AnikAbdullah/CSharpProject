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
    public partial class AdminPanel : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

        public AdminPanel()
        {
            InitializeComponent();

            this.Load += AdminPanel_Load;

            tabCustomers.Enter += tabCustomers_Enter;
            tabAdmins.Enter += tabAdmins_Enter;
            tabProducts.Enter += tabProducts_Enter;
            tabOrders.Enter += tabOrders_Enter;
            tabCart.Enter += tabCart_Enter;
            dgvCustomers.Font = new Font("Arial", 12, FontStyle.Regular);

        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
            LoadAdminData();
            LoadProductData();
            LoadOrderData();
            LoadCartData();
        }


        // Event handlers for tab page entry
        private void tabCustomers_Enter(object sender, EventArgs e) => LoadCustomerData();
        private void tabAdmins_Enter(object sender, EventArgs e) => LoadAdminData();
        private void tabProducts_Enter(object sender, EventArgs e) => LoadProductData();
        private void tabOrders_Enter(object sender, EventArgs e) => LoadOrderData();
        private void tabCart_Enter(object sender, EventArgs e) => LoadCartData();


        // Load Data Methods
        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CustomerInfo", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }

        private void LoadAdminData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AdminInfo", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAdmins.DataSource = dt;
            }
        }

        private void LoadProductData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProducts.DataSource = dt;
            }
        }

        private void LoadOrderData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Orders", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvOrders.DataSource = dt;
            }
        }

        private void LoadCartData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cart", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCart.DataSource = dt;
            }
        }


        // Similar button click events for Admins, Products, Orders, Cart, and Users...

        // Button click events for Admins
        private void btnAddAdmin_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnUpdateAdmin_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnDeleteAdmin_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnSearchAdmin_Click(object sender, EventArgs e) { /* Implement */ }

        // Button click events for Products
        private void btnAddProduct_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnUpdateProduct_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnDeleteProduct_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnSearchProduct_Click(object sender, EventArgs e) { /* Implement */ }

        // Button click events for Orders
        private void btnAddOrder_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnUpdateOrder_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnDeleteOrder_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnSearchOrder_Click(object sender, EventArgs e) { /* Implement */ }

        // Button click events for Cart
        private void btnDeleteFromCart_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnCheckout_Click(object sender, EventArgs e) { /* Implement */ }

        // Button click events for Users
        private void btnAddUser_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnUpdateUser_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnDeleteUser_Click(object sender, EventArgs e) { /* Implement */ }
        private void btnSearchUser_Click(object sender, EventArgs e) { /* Implement */ }


        // For Customer
        // Add Customer
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            // Open a form for adding a new customer
            AddCustomerForm addCustomerForm = new AddCustomerForm();
            if (addCustomerForm.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerData();
            }
        }

        // Delete Customer
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var selectedRow = dgvCustomers.SelectedRows[0];
                var customerId = (int)selectedRow.Cells["CustomerID"].Value; // Assuming there's a CustomerID column

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM CustomerInfo WHERE CustomerID = @CustomerID", conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadCustomerData();
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }

        // Search Customer
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchCustomer.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Updated SQL command to search by ID, Name, or Email
                SqlCommand cmd = new SqlCommand("SELECT * FROM CustomerInfo WHERE CustomerID LIKE @SearchText OR Name LIKE @SearchText OR Email LIKE @SearchText", conn);

                // Use the same parameter for all three fields with a wildcard
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the results to the DataGridView
                dgvCustomers.DataSource = dt;
            }
        }

        // Update Customer 
        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0) // Ensure a row is selected
            {
                // Assuming the CustomerID is in the first cell of the selected row
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value);

                // Create an instance of UpdateCustomer and pass the selected customerId
                UpdateCustomer updateCustomerForm = new UpdateCustomer(customerId);
                updateCustomerForm.ShowDialog(); // Open the form modally

                // Reload customer data after the update
                LoadCustomerData();
            }
            else
            {
                MessageBox.Show("Please select a customer to update.");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        // For Products
        // Add Products
        private void btnAddProducts_Click(object sender, EventArgs e)
        {
            // Open the AddProductForm to add a new product
            AddProductForm addProductForm = new AddProductForm();
            if (addProductForm.ShowDialog() == DialogResult.OK)
            {
                LoadProductData(); // Reload product data after adding a new product
            }
        }

        private void btnDeleteProducts_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                var productId = (int)selectedRow.Cells["ProductID"].Value; // Assuming there's a ProductID column

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadProductData(); // Reload product data after deletion
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchProducts.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductName LIKE @SearchText OR ProductColor LIKE @SearchText", conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the results to the DataGridView
                dgvProducts.DataSource = dt;
            }
        }

        private void btnUpdateProducts_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0) // Ensure a row is selected
            {
                // Assuming the ProductID is in the first cell of the selected row
                int productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);

                // Create an instance of UpdateProduct and pass the selected productId
                UpdateProduct updateProductForm = new UpdateProduct(productId);
                updateProductForm.ShowDialog(); // Open the form modally

                // Reload product data after the update
                LoadProductData();
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }
    }
}

