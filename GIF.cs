using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Media_Sphere
{
    public class GIF
    {
        public static void DisplayGIFImage(string imagePath, Border mediaDisplayBorder)
        {
            try
            {
                Image image = new Image();

                BitmapImage gifImage = new BitmapImage();
                gifImage.BeginInit();
                gifImage.UriSource = new Uri(imagePath);
                gifImage.EndInit();

                ImageBehavior.SetAnimatedSource(image, gifImage);

                // Replace the content in the Media Display Area with the Image control
                mediaDisplayBorder.Child = image;
            }
            catch (Exception ex)
            {
                // Handle the exception (display an error message, log, etc.)
                Console.WriteLine($"Error displaying GIF image: {ex.Message}");
            }
        }
    }
}

