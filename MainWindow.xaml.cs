using Media_Sphere;
using NAudio.Wave;
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
        private FLACPlayer flacPlayer;

        public MainWindow()
        {
            InitializeComponent();
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
            isMenuOpen = !isMenuOpen;
            DropdownMenu.IsOpen = isMenuOpen;

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
            openFileDialog.Filter = "All files (*.*)|*.*";
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
                else if (extension.Equals(".flac", StringComparison.OrdinalIgnoreCase))
                {
                    flacPlayer = new FLACPlayer();
                    flacPlayer.PlayFLAC(filePath);

                    DisplayAudioControlPanel("Pause/Play", flacPlayer.PauseOrResume, flacPlayer.Replay);
                    return;
                }
                else if (extension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    WAVPlayer wavPlayer = new WAVPlayer();
                    wavPlayer.PlayWAV(filePath);

                    DisplayAudioControlPanel("Pause/Play", wavPlayer.PauseOrResume, wavPlayer.Replay);
                    return;
                }
                else if (extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    MP3Player mp3Player = new MP3Player();
                    mp3Player.PlayMP3(filePath);

                    DisplayAudioControlPanel("Pause/Play", mp3Player.PauseOrResume, mp3Player.Replay);
                    return;
                }
                else if (extension.Equals(".wmv", StringComparison.OrdinalIgnoreCase))
                {
                    WMVPlayer wmvPlayer = new WMVPlayer();
                    wmvPlayer.PlayWMV(filePath);

                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", wmvPlayer.PauseOrResume, wmvPlayer.Replay, wmvPlayer.GetMediaElement());
                    return;
                }
                else if (extension.Equals(".mov", StringComparison.OrdinalIgnoreCase))
                {
                    MOVPlayer movPlayer = new MOVPlayer();
                    movPlayer.PlayMOV(filePath);

                    // Use the CreateVideoControlPanel method from MainWindow to display MOV content
                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", movPlayer.PauseOrResume, movPlayer.Replay, movPlayer.GetMediaElement());
                    return;
                }
                else if (extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    MP4Player mp4Player = new MP4Player();
                    mp4Player.PlayMP4(filePath);


                    MediaDisplayBorder.Child = CreateVideoControlPanel("Pause/Play", mp4Player.PauseOrResume, mp4Player.Replay, mp4Player.GetMediaElement());
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
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                Image image = new Image();
                image.Source = bitmapImage;

                MediaDisplayBorder.Child = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying PNG image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayTextContent(string content)
        {
            MediaDisplayBorder.Background = Brushes.White;

            FlowDocumentReader flowDocumentReader = new FlowDocumentReader();

            FlowDocument flowDocument = new FlowDocument();

            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                Paragraph paragraph = new Paragraph(new Run(line));

                paragraph.FontFamily = new FontFamily("Calibri");
                paragraph.FontSize = 12;

                flowDocument.Blocks.Add(paragraph);
            }

            flowDocumentReader.Document = flowDocument;

            flowDocumentReader.ViewingMode = FlowDocumentReaderViewingMode.Scroll;

            MediaDisplayBorder.Child = flowDocumentReader;
        }
    }
}
