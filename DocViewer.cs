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
        //Creates a MessageBox by reading the given .txt file path
        public void Display(string input)
        {
            MessageBox.Show(File.ReadAllText(input));
        }
    }
}
