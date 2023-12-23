using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Media_Sphere.ImageClasses
{
    public class JPEG
    {
        public static void DisplayJPEGImage(string imagePath, Border mediaDisplayBorder)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                Image image = new Image();
                image.Source = bitmapImage;

                // Replace the content in the Media Display Area with the Image control
                mediaDisplayBorder.Child = image;
            }
            catch (Exception ex)
            {
                // Handle the exception (display an error message, log, etc.)
                Console.WriteLine($"Error displaying JPEG image: {ex.Message}");
            }
        }
    }
}
