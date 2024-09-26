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
    public partial class MyOrders : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";

        public MyOrders()
        {
            InitializeComponent();
            LoadOrders(); // Load orders on form load
        }

        private void LoadOrders()
        {
            string query = @"
        SELECT o.OrderID, o.UserName, o.DeliveryAddress, o.MobileNumber, o.PaymentMethod, o.OrderTime, o.EstimatedDeliveryTime 
        FROM Orders o 
        WHERE o.CustomerID = @CustomerID"; // Assuming you want to filter by CustomerID

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID); // Filter by CustomerID
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable orderData = new DataTable();
                adapter.Fill(orderData); // Fill DataTable with order data

                dataGridViewOrders.DataSource = orderData; // Bind DataTable to DataGridView
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            this.Hide();
            cart.Show();
        }
    }
}
