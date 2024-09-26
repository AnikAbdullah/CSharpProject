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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace StepUp
{
    public partial class Products : Form
    {
        public class ProductInfo
        {
            public int ProductID { get; set; } // Add this line
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Color { get; set; }
            public int Quantity { get; set; }
            public int Size { get; set; }
        }

        public Products()
        {
            InitializeComponent();

            pictureBoxColor1.Click += pictureBoxColor1_Click;
            pictureBoxColor2.Click += pictureBoxColor2_Click;
            pictureBoxColor3.Click += pictureBoxColor3_Click;
            pictureBoxColor4.Click += pictureBoxColor4_Click;
            pictureBoxColor5.Click += pictureBoxColor5_Click;

            LoadInitialImage();
        }
        private void LoadInitialImage()
        {
            // Load the initial images for all PictureBoxes
            try
            {
                displayPictureBox1.Image = Image.FromFile(ImageManager.Paths["Shoe1"]);
                displayPictureBox2.Image = Image.FromFile(ImageManager.Paths["Shoe3"]);
                displayPictureBox3.Image = Image.FromFile(ImageManager.Paths["Shoe4"]);

                pictureBoxColor1.Image = Image.FromFile(ImageManager.Paths["Shoe1"]);
                pictureBoxColor2.Image = Image.FromFile(ImageManager.Paths["Shoe2"]);
                pictureBoxColor3.Image = Image.FromFile(ImageManager.Paths["Shoe3"]);
                pictureBoxColor4.Image = Image.FromFile(ImageManager.Paths["Shoe4"]);
                pictureBoxColor5.Image = Image.FromFile(ImageManager.Paths["Shoe5"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }


        private void pictureBoxColor1_Click(object sender, EventArgs e)
        {
            // Load Shoe1 image when pictureBoxColor1 is clicked
            try
            {
                displayPictureBox1.Image = Image.FromFile(ImageManager.Paths["Shoe1"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Shoe1 image: {ex.Message}");
            }
        }

        private void pictureBoxColor2_Click(object sender, EventArgs e)
        {
            // Load Shoe2 image when pictureBoxColor2 is clicked
            try
            {
                displayPictureBox1.Image = Image.FromFile(ImageManager.Paths["Shoe2"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Shoe2 image: {ex.Message}");
            }
        }

        private void pictureBoxColor3_Click(object sender, EventArgs e)
        {
            // Load Shoe2 image when pictureBoxColor2 is clicked
            try
            {
                displayPictureBox2.Image = Image.FromFile(ImageManager.Paths["Shoe3"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Shoe2 image: {ex.Message}");
            }
        }
        private void pictureBoxColor4_Click(object sender, EventArgs e)
        {
            // Load Shoe2 image when pictureBoxColor2 is clicked
            try
            {
                displayPictureBox3.Image = Image.FromFile(ImageManager.Paths["Shoe4"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Shoe2 image: {ex.Message}");
            }
        }
        private void pictureBoxColor5_Click(object sender, EventArgs e)
        {
            // Load Shoe2 image when pictureBoxColor2 is clicked
            try
            {
                displayPictureBox3.Image = Image.FromFile(ImageManager.Paths["Shoe5"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Shoe2 image: {ex.Message}");
            }
        }

        private void guna2Button22_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void displayPictureBox1_MouseHover(object sender, EventArgs e)
        {
            HoverPanel1.Visible = true;
            AddToCart1.Visible = true;
        }

        private void HoverPanel1_MouseLeave(object sender, EventArgs e)
        {
            HoverPanel1.Visible = false;
            AddToCart1.Visible = false;
        }

        private void displayPictureBox2_AMouseHover(object sender, EventArgs e)
        {
            HoverPanel2.Visible = true;
            AddToCart2.Visible = true;
        }

        private void HoverPanel2_MouseLeave_1(object sender, EventArgs e)
        {
            HoverPanel2.Visible = false;
            AddToCart2.Visible = false;
        }

        private void displayPictureBox3_MouseHover(object sender, EventArgs e)
        {
            HoverPanel3.Visible = true;
            AddToCart3.Visible = true;
        }

        private void HoverPanel3_MouseLeave(object sender, EventArgs e)
        {
            HoverPanel3.Visible = false;
            AddToCart3.Visible = false;
        }

        private void AddToCart1_Click(object sender, EventArgs e)
        {
            ConfirmPanel.Visible = true;
            ColorPanel1.Visible = true;
            ColorPanel2.Visible = false;
            ColorPanel3.Visible = false;
            ConfirmButton1.Visible = true;

        }

        private void AddToCart2_Click(object sender, EventArgs e)
        {
            ConfirmPanel.Visible = true;
            ColorPanel2.Visible = true;
            ColorPanel1.Visible = false;
            ColorPanel3.Visible = false;
            ConfirmButton1.Visible = true;

        }

        private void AddToCart3_Click(object sender, EventArgs e)
        {
            ConfirmPanel.Visible = true;
            ColorPanel3.Visible = true;
            ColorPanel2.Visible = false;
            ColorPanel1.Visible = false; 
            ConfirmButton1.Visible = true;

        }

        private int GetProductTag()
        {
            if (radioButton1.Checked)
                return 201;
            else if (radioButton2.Checked)
                return 202;
            else if (radioButton3.Checked)
                return 203;
            else if (radioButton5.Checked)
                return 204;
            else if (radioButton6.Checked)
                return 205;

            return 0; // No product selected
        }

        private void ConfirmButton1_Click(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (UserSession.CustomerID == 0)
            {
                MessageBox.Show("You must be logged in to add products to the cart.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected tag from the checked radio button
            int selectedTag = GetProductTag();

            // Check if a tag was found
            if (selectedTag == 0)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            // Get quantity and size from user input
            int selectedQuantity = (int)numericUpDown.Value;
            int selectedSize = Convert.ToInt32(comboBoxSize.SelectedItem);

            // Fetch product details based on the selected tag from the database
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\ABDUL\ONEDRIVE\DOCUMENTS\STEPUPDB.MDF;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT ProductID, ProductName, ProductPrice, ProductColor, AvailableQuantity FROM Products WHERE Tag = @Tag";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Fetch product details
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tag", selectedTag);

                int productID;
                string productName;
                decimal productPrice;
                string productColor;
                int availableQuantity;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Get product details
                        productID = Convert.ToInt32(reader["ProductID"]);
                        productName = reader["ProductName"].ToString();
                        productPrice = Convert.ToDecimal(reader["ProductPrice"]);
                        productColor = reader["ProductColor"].ToString();
                        availableQuantity = Convert.ToInt32(reader["AvailableQuantity"]); // Fetch available quantity
                    }
                    else
                    {
                        MessageBox.Show("No product found with the selected tag.");
                        return; // Exit if no product is found
                    }
                } // The reader is closed here

                // Check if the requested quantity is greater than available
                if (selectedQuantity > availableQuantity)
                {
                    MessageBox.Show($"You cannot add more than {availableQuantity} of this product to your cart.", "Quantity Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit if the quantity is exceeded
                }

                // Start a transaction for safe insert and update
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Add product details to the Cart database
                        string insertQuery = "INSERT INTO Cart (ProductID, CustomerID, Quantity, ProductName, Color, Size, UserName) VALUES (@ProductID, @CustomerID, @Quantity, @ProductName, @Color, @Size, @UserName)";

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@ProductID", productID);
                            insertCmd.Parameters.AddWithValue("@CustomerID", UserSession.CustomerID);
                            insertCmd.Parameters.AddWithValue("@Quantity", selectedQuantity);
                            insertCmd.Parameters.AddWithValue("@ProductName", productName);
                            insertCmd.Parameters.AddWithValue("@Color", productColor);
                            insertCmd.Parameters.AddWithValue("@Size", selectedSize);
                            insertCmd.Parameters.AddWithValue("@UserName", UserSession.UserName); // Assuming UserSession.UserName holds the customer's name

                            // Execute the insert command
                            insertCmd.ExecuteNonQuery();
                        }

                        // Update the available quantity in the Products table
                        string updateQuery = "UPDATE Products SET AvailableQuantity = AvailableQuantity - @Quantity WHERE ProductID = @ProductID";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@Quantity", selectedQuantity);
                            updateCmd.Parameters.AddWithValue("@ProductID", productID);

                            // Execute the update command
                            updateCmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        MessageBox.Show("Product added to cart successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void guna2ControlBox4_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            this.Visible = false;
            products.Show();
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            MyOrders M = new MyOrders();
            this.Hide();
            M.Show();
        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            this.Hide();
            cart.Show();
        }
    }
}