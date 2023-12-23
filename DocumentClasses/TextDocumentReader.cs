using System.IO;

namespace Media_Sphere.DocumentClasses
{
    public class TextDocumentReader
    {
        public static string ReadTextFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
