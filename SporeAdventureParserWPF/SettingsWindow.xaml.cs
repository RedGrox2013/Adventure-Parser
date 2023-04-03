using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace SporeAdventureParserWPF
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private bool _hasChanges = false;

        static private bool _playBoyaroshnik;
        static public bool PlayBoyaroshnik => _playBoyaroshnik;
        static SettingsWindow()
        {
            try
            {
                _playBoyaroshnik = bool.Parse(File.ReadAllLines("PlayBoyaroshnik")[0]);
            }
            catch
            {
                _playBoyaroshnik = true;
                WriteBoyaroshnik();
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();

            pathBox.Text = CreationDownloader.MySporeCreationsPath;
            playBoyaroshnikCheckBox.IsChecked = _playBoyaroshnik;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_hasChanges)
            {
                CreationDownloader.MySporeCreationsPath = pathBox.Text;
                _playBoyaroshnik = playBoyaroshnikCheckBox.IsChecked ?? false;
                WriteBoyaroshnik();
            }

            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void cancelBtn_Click(object sender, RoutedEventArgs e) => Close();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog
            {
                Description = "Выберите папку для загрузки творений"
            };
            var result = folderBrowser.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK &&
                !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                pathBox.Text = folderBrowser.SelectedPath;
                _hasChanges = true;
            }
        }

        private static void WriteBoyaroshnik() => 
            File.WriteAllText("PlayBoyaroshnik", _playBoyaroshnik.ToString());

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void autoBtn_Click(object sender, RoutedEventArgs e) => pathBox.Text = CreationDownloader.AutoDetect();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void playBoyaroshnikCheckBox_Click(object sender, RoutedEventArgs e) => _hasChanges = true;
    }
}
