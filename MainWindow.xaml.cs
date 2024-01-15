using Media_Sphere.DocumentClasses;
using Media_Sphere.ImageClasses;
using Media_Sphere.VideoClasses;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private SQLiteSchema sqliteSchema;
        private FLACPlayer flacPlayer;

        public MainWindow()
        {
            InitializeComponent();
            sqliteSchema = new SQLiteSchema();
            flacPlayer = new FLACPlayer();
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
            //openFileDialog.Filter = "Word files (*.docx;*.doc)|*.docx;*.doc|PNG files (*.png)|*.png|Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            openFileDialog.Filter = "All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.InitialDirectory = "test_data";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = string.Empty;

                string extension = System.IO.Path.GetExtension(filePath);

                if (extension.Equals(".docx") || extension.Equals(".doc"))
                {
                    fileContent = WordDocumentReader.ReadTextFromWordDocument(filePath);
                    sqliteSchema.AddRecentFile(filePath);
                }
                else if (extension.Equals(".txt"))
                {
                    fileContent = TextDocumentReader.ReadTextFromFile(filePath);
                    sqliteSchema.AddRecentFile(filePath);
                }
                else if (extension.Equals(".pdf"))
                {
                    fileContent = PdfDocumentReader.ReadTextFromPdfDocument(filePath);
                    sqliteSchema.AddRecentFile(filePath);
                }
                else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayImage(filePath);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    JPEG.DisplayJPEGImage(filePath, MediaDisplayBorder);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    GIF.DisplayGIFImage(filePath, MediaDisplayBorder);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".flac", StringComparison.OrdinalIgnoreCase))
                {
                    flacPlayer = new FLACPlayer();
                    flacPlayer.PlayFLAC(filePath);

                    DisplayAudioControlPanel("Pause/Play", flacPlayer.PauseOrResume, flacPlayer.Replay);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    WAVPlayer wavPlayer = new WAVPlayer();
                    wavPlayer.PlayWAV(filePath);

                    DisplayAudioControlPanel("Pause/Play", wavPlayer.PauseOrResume, wavPlayer.Replay);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    MP3Player mp3Player = new MP3Player();
                    mp3Player.PlayMP3(filePath);

                    DisplayAudioControlPanel("Pause/Play", mp3Player.PauseOrResume, mp3Player.Replay);
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".wmv", StringComparison.OrdinalIgnoreCase))
                {
                    WMVPlayer wmvPlayer = new WMVPlayer();
                    wmvPlayer.PlayWMV(filePath);

                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", wmvPlayer.PauseOrResume, wmvPlayer.Replay, wmvPlayer.GetMediaElement());
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".mov", StringComparison.OrdinalIgnoreCase))
                {
                    MOVPlayer movPlayer = new MOVPlayer();
                    movPlayer.PlayMOV(filePath);

                    // Use the CreateVideoControlPanel method from MainWindow to display MOV content
                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", movPlayer.PauseOrResume, movPlayer.Replay, movPlayer.GetMediaElement());
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else if (extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    MP4Player mp4Player = new MP4Player();
                    mp4Player.PlayMP4(filePath);


                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", mp4Player.PauseOrResume, mp4Player.Replay, mp4Player.GetMediaElement());
                    sqliteSchema.AddRecentFile(filePath);
                    return;
                }
                else
                {
                    MessageBox.Show("Unsupported file format");
                    return;
                }

                DisplayTextContent(fileContent);
                sqliteSchema.AddRecentFile(filePath);
            }
        }

        // Recent File Button
        private bool isRecentFilesMenuOpen = false;

        public void CollapseRecentFilesMenu()
        {
            RecentFilesMenu.IsOpen = false;
            isRecentFilesMenuOpen = false;
        }

        public void RecentFilesButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve recent files from the database
            string[] recentFiles = sqliteSchema.GetRecentFiles();

            // Initialize and apply the RecentFilesMenuStyle
            RecentFilesMenu = new Popup();
            RecentFilesMenu.Style = (Style)FindResource("RecentFilesMenuStyle");

            // Create the content for the RecentFilesMenu
            StackPanel recentFilesPanel = new StackPanel();

            // Add the recent files as menu items
            foreach (string filePath in recentFiles)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = System.IO.Path.GetFileName(filePath);
                menuItem.Click += (s, args) => OpenFile(filePath);
                recentFilesPanel.Children.Add(menuItem);
            }

            // Set the content to the Popup
            RecentFilesMenu.Child = recentFilesPanel;

            // Set placement and open the RecentFilesMenu
            RecentFilesMenu.PlacementTarget = HamburgerButton;
            RecentFilesMenu.IsOpen = true;
        }




        // Method to handle opening the file associated with a clicked menu item
        private void OpenFile(string filePath)
        {
            // Your code to open/display the file based on its filePath
            // This might involve different logic depending on the file type.
            // For example, you might have methods for opening different file types.
        }


        // DisplayAudioControlPanel method
        private void DisplayAudioControlPanel(string buttonText, Action playAction, Action replayAction)
        {
            MediaDisplayBorder.Background = Brushes.Black;

            StackPanel buttonPanel = new StackPanel();
            buttonPanel.HorizontalAlignment = HorizontalAlignment.Center;
            buttonPanel.VerticalAlignment = VerticalAlignment.Center;

            Button pausePlayButton = new Button();
            pausePlayButton.Content = buttonText;
            pausePlayButton.Click += (s, args) => playAction();
            buttonPanel.Children.Add(pausePlayButton);

            Button replayButton = new Button();
            replayButton.Content = "Replay";
            replayButton.Click += (s, args) => replayAction();
            buttonPanel.Children.Add(replayButton);

            MediaDisplayBorder.Child = buttonPanel;
        }

        public UIElement CreateVideoControlPanel(string buttonText, Action playAction, Action replayAction, MediaElement mediaElement)
        {
            StackPanel buttonPanel = new StackPanel();

            Button pausePlayButton = new Button();
            pausePlayButton.Content = buttonText;
            pausePlayButton.Click += (s, args) => playAction();
            buttonPanel.Children.Add(pausePlayButton);

            Button replayButton = new Button();
            replayButton.Content = "Replay";
            replayButton.Click += (s, args) => replayAction();
            buttonPanel.Children.Add(replayButton);

            mediaElement.MediaEnded += (s, args) =>
            {
                mediaElement.Stop();
                replayAction?.Invoke();
            };

            StackPanel panelWithMedia = new StackPanel();
            panelWithMedia.Children.Add(buttonPanel);
            panelWithMedia.Children.Add(mediaElement);

            return panelWithMedia;
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