using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StepUp
{
    public partial class Form1 : Form
    {
        private string[] images;
        private int currentIndex = 1;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;  // Set picture box to stretch images
            images = ImageManager.LoadSlideshowImages();  // Call to load images from ImageManager
            StartSlideshow();  // Start the slideshow
        }

        private void StartSlideshow()
        {
            if (images.Length == 0)
            {
                MessageBox.Show("No valid images to display.");
                return;
            }

            // Load the first image with error handling
            try
            {
                pictureBox1.Image = Image.FromFile(images[currentIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }

            // Set timer interval and start it
            timer1.Interval = 2000;  // 2 seconds interval
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Move to the next image
            currentIndex++;
            if (currentIndex >= images.Length)
            {
                currentIndex = 0;  // Loop back to the first image
            }

            // Load the current image with error handling
            try
            {
                pictureBox1.Image = Image.FromFile(images[currentIndex]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
            }
        }

        private void guna2Button10_Click_1(object sender, EventArgs e)
        {
            Products P1 = new Products();
            P1.Show();
            this.Hide();
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            MyOrders B = new MyOrders();
            B.Show();
            this.Hide();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L.Show();
            this.Hide();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            this.Hide();
            cart.Show();    
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            StoreLocator storeLocator = new StoreLocator();
            this.Hide();
            storeLocator.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
