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

        private const string SETTINGS_FILE_NAME = "PlayBoyaroshnik";

        static SettingsWindow()
        {
            try
            {
                _playBoyaroshnik = bool.Parse(File.ReadAllLines(SETTINGS_FILE_NAME)[0]);
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
            engFoldersCheckBox.IsChecked = CreationDownloader.IsEnglishFoldersNames;
        }

#pragma warning disable IDE1006 // Стили именования
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

        private void cancelBtn_Click(object sender, RoutedEventArgs e) => Close();

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
                MakeChange(sender, e);
            }
        }

        private static void WriteBoyaroshnik() => 
            File.WriteAllText(SETTINGS_FILE_NAME, _playBoyaroshnik.ToString());

        private void autoBtn_Click(object sender, RoutedEventArgs e) => pathBox.Text = CreationDownloader.AutoDetect();

        private void MakeChange(object sender, RoutedEventArgs e) => _hasChanges = true;

        private void engFoldersCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CreationDownloader.IsEnglishFoldersNames = (bool)engFoldersCheckBox.IsChecked;
            MakeChange(sender, e);
        }
#pragma warning restore IDE1006 // Стили именования
    }
}
