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

namespace Media_Sphere
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMenuOpen = false;

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            isMenuOpen = !isMenuOpen; // Toggle the menu state
            DropdownMenu.IsOpen = isMenuOpen; // Set menu visibility based on the state

            // If the menu is closed, call CollapseMenu
            if (!isMenuOpen)
            {
                CollapseMenu();
            }
        }

        private void CollapseMenu()
        {
            DropdownMenu.IsOpen = false;
            isMenuOpen = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CollapseMenu();
        }


        private void HamburgerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DropdownMenu.Visibility = Visibility.Visible;
            isMenuOpen = true;
        }

        private void DropdownButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
        }

        private void DropdownButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }

        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Word files (*.docx;*.doc)|*.docx;*.doc|PNG files (*.png)|*.png|Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = string.Empty;

                string extension = System.IO.Path.GetExtension(filePath);

                if (extension.Equals(".docx") || extension.Equals(".doc"))
                {
                    fileContent = WordDocumentReader.ReadTextFromWordDocument(filePath);
                }
                else if (extension.Equals(".txt"))
                {
                    fileContent = TextDocumentReader.ReadTextFromFile(filePath);
                }
                else if (extension.Equals(".pdf"))
                {
                    fileContent = PdfDocumentReader.ReadTextFromPdfDocument(filePath);
                }
                else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayImage(filePath);
                    return;
                }
                else if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    JPEG.DisplayJPEGImage(filePath, MediaDisplayBorder);
                    return;
                }
                else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    GIF.DisplayGIFImage(filePath, MediaDisplayBorder);
                    return;
                }
                else
                {
                    MessageBox.Show("Unsupported file format");
                    return;
                }

                DisplayTextContent(fileContent);
            }
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                // Read and display the PNG image using an Image control
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                Image image = new Image();
                image.Source = bitmapImage;

                // Replace the content in the Media Display Area with the Image control
                MediaDisplayBorder.Child = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying PNG image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DisplayTextContent(string content)
        {
            // Change the background color to white for better text visibility
            MediaDisplayBorder.Background = Brushes.White;

            // Display the text content in a FlowDocumentReader to retain formatting
            FlowDocumentReader flowDocumentReader = new FlowDocumentReader();

            // Create a FlowDocument and add Paragraphs for each line of the content
            FlowDocument flowDocument = new FlowDocument();

            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                Paragraph paragraph = new Paragraph(new Run(line));

                // Set text properties for better readability
                paragraph.FontFamily = new FontFamily("Calibri"); // Use a suitable font
                paragraph.FontSize = 12; // Set the font size

                flowDocument.Blocks.Add(paragraph);
            }

            flowDocumentReader.Document = flowDocument;

            // Set the default view mode to scroll
            flowDocumentReader.ViewingMode = FlowDocumentReaderViewingMode.Scroll;

            // Replace the content in the Media Display Area with the FlowDocumentReader
            MediaDisplayBorder.Child = flowDocumentReader;
        }
    }
}