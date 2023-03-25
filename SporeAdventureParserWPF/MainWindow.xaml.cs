using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SporeAdventureParserWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource _defaultBtnPicture,
            _mouseEnterBtnPicture, _clickBtnPicture;

        public MainWindow()
        {
            ReadCommandLineArgs();
            InitializeComponent();

            _defaultBtnPicture = imageDwnldBtn.Source;
            _mouseEnterBtnPicture = new BitmapImage(
                new Uri("pack://application:,,,/images/aaa-krtaya-koshachka_MouseEnter.png"));
            _clickBtnPicture = new BitmapImage(
                new Uri("pack://application:,,,/images/aaa-krtaya-koshachka_Click.png"));
        }

        private void ReadCommandLineArgs()
        {
            string[] tmp = Environment.GetCommandLineArgs();
            if (tmp.Length <= 1)
                return;

            string[] args = new string[tmp.Length - 1];
            Array.Copy(tmp, 1, args, 0, args.Length);
            DownloadAdventure(args);

            Close();
        }

        private void DownloadAdventure(params string[] uri)
        {
            var downloaderWin = new DownloadProgressWindow();
            downloaderWin.DownloadAdventure(uri);
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputBox.Text.StartsWith("http"))
                inputBox.Foreground = Brushes.Blue;
            else
                inputBox.Foreground = Brushes.Black;
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            imageDwnldBtn.Source = _clickBtnPicture;
            DownloadAdventure(inputBox.Text.Split(' '));
        }
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DownloadAdventure(inputBox.Text.Split(' '));
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            var aboutWin = new AboutWindow();
            try
            {
                aboutWin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImageBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            imageDwnldBtn.Source = _clickBtnPicture;

        private void ImageBtn_MouseEnter(object sender, MouseEventArgs e) =>
            imageDwnldBtn.Source = _mouseEnterBtnPicture;

        private void ImageBtn_MouseLeave(object sender, MouseEventArgs e) =>
            imageDwnldBtn.Source = _defaultBtnPicture;
    }
}
