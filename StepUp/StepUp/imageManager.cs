using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StepUp
{
    internal class ImageManager
    {
        // First dictionary for shoes
        public static Dictionary<string, string> Paths = new Dictionary<string, string>
        {
            {"Shoe1", @"D:\CSharpProject\StepUp\StepUp\Images\AJZCMFT.png"},
            {"Shoe2", @"D:\CSharpProject\StepUp\StepUp\Images\AJZCMFT2.png"},
            {"Shoe3", @"D:\CSharpProject\StepUp\StepUp\Images\Shoe1Ls.png"},
            {"Shoe4", @"D:\CSharpProject\StepUp\StepUp\Images\JR6_L.png"},
            {"Shoe5", @"D:\CSharpProject\StepUp\StepUp\Images\JR6B_L.png"}
        };

        // Second dictionary for other images
        public static Dictionary<string, string> SlideshowImages = new Dictionary<string, string>
        {
            {"Image1", @"D:\CSharpProject\StepUp\StepUp\images1.jpg"},
            {"Image2", @"D:\CSharpProject\StepUp\StepUp\images2.jpg"},
            {"Image3", @"D:\CSharpProject\StepUp\StepUp\images3.jpg"},
            {"Image4", @"D:\CSharpProject\StepUp\StepUp\images4.jpg"},
            {"Image5", @"D:\CSharpProject\StepUp\StepUp\images5.jpg"},
            {"Image6", @"D:\CSharpProject\StepUp\StepUp\images6.jpg"},
            {"Image7", @"D:\CSharpProject\StepUp\StepUp\images7.png"}
        };

        // Function to load and validate slideshow images
        public static string[] LoadSlideshowImages()
        {
            List<string> validImages = new List<string>();

            foreach (var imagePath in SlideshowImages.Values)
            {
                if (File.Exists(imagePath) && IsValidImage(imagePath))
                {
                    validImages.Add(imagePath);
                }
                else
                {
                    MessageBox.Show($"Error: Image file '{imagePath}' is either missing or invalid.");
                }
            }

            return validImages.ToArray(); // Return valid image paths
        }

        // Validate image files before loading them
        private static bool IsValidImage(string imagePath)
        {
            try
            {
                using (var img = Image.FromFile(imagePath))
                {
                    return img != null; // Return true if the image is valid
                }
            }
            catch
            {
                return false; // Return false if the image cannot be loaded
            }
        }
    }
}
