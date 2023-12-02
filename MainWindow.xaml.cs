using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Media_Sphere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Creates instances of AudioPlayer and VideoPlayer
        private AudioPlayer ap;
        private VideoPlayer vp;

        public MainWindow()
        {
            InitializeComponent();
            //Loads AudioPlayer and VideoPlayer with the media element inputMdEl
            ap = new AudioPlayer(inputMdEl);
            vp = new VideoPlayer(inputMdEl);
        }

        private void inputBtn_Click(object sender, RoutedEventArgs e)
        {
            //Sends user inputted file path to FileSort
            FileSort(inputTxb.Text);
        }

        public void FileSort(string input)
        {
            //Creates instance of DocViewer
            DocViewer dc = new DocViewer();

            //Ensures given file path contains valid characters and that the file exists
            if (input.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
                MessageBox.Show("Input contains invalid characters.");
            else if (!File.Exists(input))
                MessageBox.Show("Input is NOT a valid file path.");
            else
            {
                //Gets extension of given file path
                string extension = System.IO.Path.GetExtension(input).ToLower();
                
                //Based on the extension, calls either DocViwer, AudioPlayer, or VideoPlayer accordingly to perform the logic
                switch (extension)
                {
                    case ".txt":
                        dc.Display(input);
                        break;
                    case ".mp3":
                        ap.Play(input);
                        break;
                    case ".mp4":
                        vp.Play(input);
                        break;
                }
            }
        }
    }
}
