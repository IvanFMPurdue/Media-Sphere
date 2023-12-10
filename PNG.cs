using System;
using System.Drawing;
using System.IO;

namespace Media_Sphere
{
    public class PNG
    { 
        public static string ReadTextFromPng(string filePath)
        {
            if (File.Exists(filePath) && Path.GetExtension(filePath).Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Read Image
                    using (Bitmap bmp = new Bitmap(filePath))
                    {

                        

                        return $"Loaded image from: {filePath}";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error reading PNG file: {ex.Message}";
                }
            }
            else
            {
                return "Invalid file or not a PNG file.";
            }
        }
    }
}
