using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Media_Sphere
{
    internal class DocViewer
    {
        public void Verify(string input)
        {
            if (input.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                MessageBox.Show("Input contains invalid characters.");
            else if (File.Exists(input) || Directory.Exists(input))
                MessageBox.Show("Input is a valid file/directory path.");
            else
                MessageBox.Show("Input is NOT a file/directory path.");
        }
    }
}
