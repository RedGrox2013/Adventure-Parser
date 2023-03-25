using System;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace SporeAdventureParserWPF
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private readonly SoundPlayer _player;

        public AboutWindow()
        {
            InitializeComponent();

            path.Text = CreationDownloader.MySporeCreationsPath;
            if (path.Text.Count(c => c == '\\') == 0)
                path.Text = Environment.CurrentDirectory + '\\' + path.Text;
            pathUri.NavigateUri = new Uri(path.Text);
            Random random = new Random();
            randomImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/info" +
                random.Next(6) + ".png"));

            _player = new SoundPlayer
            {
                SoundLocation = "..\\..\\info.wav"
            };
            _player.Play();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _player.Stop();
            _player.Dispose();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e) =>
            System.Diagnostics.Process.Start(((Hyperlink)sender).NavigateUri.ToString());
    }
}
