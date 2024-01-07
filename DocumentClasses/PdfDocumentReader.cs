using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Text;

namespace Media_Sphere.DocumentClasses
{
    public class PdfDocumentReader
    {
        public static string ReadTextFromPdfDocument(string filePath)
        {
            StringBuilder text = new StringBuilder();

            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }
                }

                return text.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error reading PDF document: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
