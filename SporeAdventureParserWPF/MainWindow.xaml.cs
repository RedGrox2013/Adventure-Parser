using SporeApi;
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
        private readonly string[] _robotChickenAdvs = { "300001213623",
            "300001213991", "300001213625", "300001213658", "300001213709",
            "300001213743", "300001213882", "300001213940", "300001213788",
            "300001213838", "300001258136" };
        private readonly string[] _maxisAdvs = { "300001190944", "300001191016",
            "300001191045", "300001191014", "300001190826", "300001191301",
            "300001125473", "300001205572", "300001219829", "300001215014",
            "300001190888", "300001215114", "300001215300", "300001191064",
            "300001205862", "501082182040", "300001191076" };

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
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
                return;

            switch (args[1].ToLower())
            {
                case "-robotchicken":
                    DownloadAdventure(_robotChickenAdvs);
                    break;
                case "-maxis":
                    DownloadAdventure(_maxisAdvs);
                    break;
                case "-redgrox":
                    DownloadRedGroxAdventures();
                    break;
                default:
                    string[] uris = new string[args.Length - 1];
                    Array.Copy(args, 1, uris, 0, uris.Length);
                    DownloadAdventure(uris);
                    break;
            }

            Close();
        }

        private void DownloadRedGroxAdventures()
        {
            // http://www.spore.com/sporepedia#qry=usr-RedGrox%7C501074940839%3Assc-501106965545
            long[] advIds = new Sporecast(501106965545).GetSporecastAssetsIDs(10000);
            string[] advIdsString = new string[advIds.Length];
            for (int i = 0; i < advIds.Length; i++)
                advIdsString[i] = advIds[i].ToString();
            DownloadAdventure(advIdsString);
        }

        private void DownloadAdventure(params string[] uri)
        {
            Hide();
            var downloaderWin = new DownloadForm(); // КРИНЖ!!!!!!!!!!
            downloaderWin.DownloadAdventure(uri);
            Visibility = Visibility.Visible;
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
            try
            {
                new AboutWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImageBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            imageDwnldBtn.Source = _clickBtnPicture;

        private void AdventuresBtns_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch ((string)btn.Content)
            {
                case "Robot Chicken":
                    DownloadAdventure(_robotChickenAdvs);
                    break;
                case "Maxis":
                    DownloadAdventure(_maxisAdvs);
                    break;
                case "RedGrox":
                    DownloadRedGroxAdventures();
                    break;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void settingsBtn_Click(object sender, RoutedEventArgs e) => new SettingsWindow().Show();

        private void ImageBtn_MouseEnter(object sender, MouseEventArgs e) =>
            imageDwnldBtn.Source = _mouseEnterBtnPicture;

        private void ImageBtn_MouseLeave(object sender, MouseEventArgs e) =>
            imageDwnldBtn.Source = _defaultBtnPicture;
    }
}
