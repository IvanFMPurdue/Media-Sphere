using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Text;

namespace Media_Sphere
{
    public class WordDocumentReader
    {
        public static string ReadTextFromWordDocument(string filePath)
        {
            StringBuilder textBuilder = new StringBuilder();

            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
                {
                    Body body = wordDoc.MainDocumentPart.Document.Body;
                    if (body != null)
                    {
                        foreach (OpenXmlElement element in body.Elements())
                        {
                            if (element is Paragraph)
                            {
                                textBuilder.AppendLine(GetParagraphText((Paragraph)element));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error reading Word document: " + ex.Message);
            }

            return textBuilder.ToString();
        }

        private static string GetParagraphText(Paragraph paragraph)
        {
            StringBuilder paragraphText = new StringBuilder();
            foreach (OpenXmlElement element in paragraph.Elements())
            {
                if (element is Run)
                {
                    foreach (Text text in element.Descendants<Text>())
                    {
                        paragraphText.Append(text.Text);
                    }
                }
                else if (element is Break)
                {
                    paragraphText.AppendLine();
                }
            }
            return paragraphText.ToString();
        }
    }
}
