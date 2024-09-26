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
using static StepUp.Products;


namespace StepUp
{

    public partial class Cart : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

        public Cart()
        {

            InitializeComponent();
            LoadCartData(); // Load cart data on form load

        }

        // Method to load the cart data into the DataGridView
        private void LoadCartData()
        {
            if (UserSession.CustomerID == 0) // Validate if the user is logged in
            {
                MessageBox.Show("You must be logged in to view your cart.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetupDataGridView(); // Set up DataGridView columns


            string query = @"
        SELECT c.ProductID, c.ProductName, c.Color, c.Size, c.Quantity, c.CustomerID, 
               ci.Name AS UserName, p.ProductPrice AS Price, 
               (c.Quantity * p.ProductPrice) AS TotalPrice
        FROM Cart c 
        INNER JOIN CustomerInfo ci ON c.CustomerID = ci.CustomerID 
        INNER JOIN Products p ON c.ProductID = p.ProductID 
        WHERE c.CustomerID = @CustomerID";

            decimal totalPrice = 0; // Total price calculation variable

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable cartData = new DataTable();
                adapter.Fill(cartData); // Fill DataTable with data

                foreach (DataRow row in cartData.Rows)
                {
                    totalPrice += Convert.ToDecimal(row["TotalPrice"]); // Calculate total price
                    dataGridViewCart.Rows.Add(
                        row["ProductID"],
                        row["ProductName"],
                        row["Color"],
                        row["Size"],
                        row["Quantity"],
                        row["CustomerID"],
                        row["UserName"],
                        row["TotalPrice"]
                    );
                }
            }

            UpdateTotalPrice(totalPrice); // Update total price
        }

        private void UpdateCart(int productID, int newQuantity, int newSize)
        {
            string updateCartQuery = "UPDATE Cart SET Quantity = @Quantity, Size = @Size WHERE ProductID = @ProductID AND CustomerID = @CustomerID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(updateCartQuery, conn);
                cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                cmd.Parameters.AddWithValue("@Size", newSize);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadCartData(); // Reload cart data after updating
        }


        // Helper method to set up the DataGridView columns
        private void SetupDataGridView()
        {
            dataGridViewCart.Columns.Clear(); // Clear existing columns

            // Add necessary columns
            dataGridViewCart.Columns.Add("ProductID", "Product ID");
            dataGridViewCart.Columns.Add("ProductName", "Product Name");
            dataGridViewCart.Columns.Add("ProductColor", "Color");
            dataGridViewCart.Columns.Add("Size", "Size");
            dataGridViewCart.Columns.Add("Quantity", "Quantity");
            dataGridViewCart.Columns.Add("CustomerID", "Customer ID");
            dataGridViewCart.Columns.Add("UserName", "User Name");
            dataGridViewCart.Columns.Add("TotalPrice", "Total Price");

            // Disable editing for all columns except Size and Quantity
            foreach (DataGridViewColumn column in dataGridViewCart.Columns)
            {
                if (column.Name != "Size" && column.Name != "Quantity")
                {
                    column.ReadOnly = true; // Make the column read-only
                }
            }
        }

        private void RecalculateTotalPrice()
        {
            decimal newTotalPrice = 0;
            foreach (DataGridViewRow row in dataGridViewCart.Rows)
            {
                newTotalPrice += Convert.ToDecimal(row.Cells["TotalPrice"].Value);
            }
            UpdateTotalPrice(newTotalPrice);
        }

        // Method to update total price label
        private void UpdateTotalPrice(decimal totalPrice)
        {
            labelTotalPrice.Text = $"Total Price: ${totalPrice:F2}"; // Format to two decimal places
        }





        // Method to handle navigating back to the main form
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.Show();
        }

        private void dataGridViewCart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = dataGridViewCart.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0)
                {
                    dataGridViewCart.ClearSelection();
                    dataGridViewCart.Rows[hitTest.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dataGridViewCart, e.Location);
                }
            }
        }

        private void dataGridViewCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewCart.Columns["Quantity"].Index || e.ColumnIndex == dataGridViewCart.Columns["Size"].Index)
            {
                int selectedRowIndex = e.RowIndex;
                int productID = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["ProductID"].Value);

                // Retrieve the new quantity and size from the DataGridView
                int newQuantity = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["Quantity"].Value);
                int newSize = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["Size"].Value); // Assuming Size is stored as an integer

                // Retrieve the available quantity of the product from the database
                int availableQuantity = GetAvailableQuantity(productID);

                if (newQuantity > availableQuantity)
                {
                    MessageBox.Show($"Insufficient quantity available. Only {availableQuantity} items are in stock.", "Quantity Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit if quantity exceeds available stock
                }

                // Retrieve the current price of the product from the database
                decimal productPrice = GetProductPrice(productID); // Method to get the price

                // Update the cart in the database
                string updateCartQuery = "UPDATE Cart SET Quantity = @Quantity, Size = @Size WHERE ProductID = @ProductID AND CustomerID = @CustomerID";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(updateCartQuery, conn);
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                    cmd.Parameters.AddWithValue("@Size", newSize);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Update the available quantity in the Products table
                UpdateAvailableQuantity(productID, newQuantity); // Call the method to update available quantity

                // Recalculate the total price for this product
                decimal newTotalPriceForProduct = productPrice * newQuantity; // Recalculate total price for this product
                dataGridViewCart.Rows[selectedRowIndex].Cells["TotalPrice"].Value = newTotalPriceForProduct; // Update the TotalPrice cell

                // Recalculate total price for all products
                RecalculateTotalPrice(); // Recalculate total price after update
                MessageBox.Show("Product updated successfully.");
            }
        }

        private void UpdateAvailableQuantity(int productID, int newQuantity)
        {
            // Get the current available quantity
            int currentAvailableQuantity = GetAvailableQuantity(productID);

            // Calculate the updated available quantity
            int updatedAvailableQuantity = currentAvailableQuantity - newQuantity;

            string updateQuery = "UPDATE Products SET AvailableQuantity = @AvailableQuantity WHERE ProductID = @ProductID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@AvailableQuantity", updatedAvailableQuantity);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private int GetAvailableQuantity(int productID)
        {
            int availableQuantity = 0;
            string query = "SELECT AvailableQuantity FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                conn.Open();
                availableQuantity = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return availableQuantity;
        }


        // Method to retrieve product price from the database
        private decimal GetProductPrice(int productID)
        {
            decimal price = 0;
            string query = "SELECT ProductPrice FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                conn.Open();
                price = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            return price;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewCart.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridViewCart.SelectedRows[0].Index;
                int productID = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["ProductID"].Value);
                int newQuantity = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["Quantity"].Value);
                int newSize = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["Size"].Value); // Assuming Size is stored as an integer

                // Update the cart in the database
                string query = "UPDATE Cart SET Quantity = @Quantity, Size = @Size WHERE ProductID = @ProductID AND CustomerID = @CustomerID";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                    cmd.Parameters.AddWithValue("@Size", newSize);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Recalculate total price for the updated product
                RecalculateTotalPrice(); // Recalculate total price after update
                MessageBox.Show("Product updated successfully.");
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCart.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridViewCart.SelectedRows[0].Index;
                int productID = Convert.ToInt32(dataGridViewCart.Rows[selectedRowIndex].Cells["ProductID"].Value);

                // Delete the selected product from the cart in the database
                string query = "DELETE FROM Cart WHERE ProductID = @ProductID AND CustomerID = @CustomerID";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Remove the selected row from the DataGridView
                dataGridViewCart.Rows.RemoveAt(selectedRowIndex);
                RecalculateTotalPrice(); // Recalculate total price after deletion
                MessageBox.Show("Product deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }

        private void buttonProceedToCheckout_Click(object sender, EventArgs e)
        {
            panelCheckout.Visible = true;
        }

        private void buttonConfirmCheckout_Click(object sender, EventArgs e)
        {
            string userName = UserSession.UserName; // Assuming UserSession holds the username
            string deliveryAddress = textBoxDeliveryAddress.Text;
            string mobileNumber = textBoxMobileNumber.Text;
            string paymentMethod = GetSelectedPaymentMethod(); // Method to get selected payment method

            if (string.IsNullOrWhiteSpace(deliveryAddress) || string.IsNullOrWhiteSpace(mobileNumber))
            {
                MessageBox.Show("Please enter both delivery address and mobile number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save order details in the database
            SaveOrderDetails(userName, deliveryAddress, mobileNumber, paymentMethod, UserSession.CustomerID);
            MessageBox.Show("Order placed successfully!");

            // Hide the checkout panel after confirming
            panelCheckout.Visible = false;
        }

        private void SaveOrderDetails(string userName, string deliveryAddress, string mobileNumber, string paymentMethod, int customerId)
        {
            string query = @"INSERT INTO Orders (UserName, DeliveryAddress, MobileNumber, PaymentMethod, OrderTime, CustomerID, EstimatedDeliveryTime) 
                     VALUES (@UserName, @DeliveryAddress, @MobileNumber, @PaymentMethod, @OrderTime, @CustomerID, @EstimatedDeliveryTime)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);
                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@OrderTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@CustomerID", customerId); // Add CustomerID
                cmd.Parameters.AddWithValue("@EstimatedDeliveryTime", DateTime.Now.AddDays(3)); // Example: 3 days delivery time

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private string GetSelectedPaymentMethod()
        {
            if (radioButtonCOD.Checked) return "COD";
            if (radioButtonCreditCard.Checked) return "Card";
            if (radioButtonPayPal.Checked) return "PayPal";
            return null;
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.Show();
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            MyOrders M = new MyOrders();
            this.Hide();
            M.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            Products products = new Products(); 
            this.Hide();
            products.Show();
        }

        private void panelCheckout_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
