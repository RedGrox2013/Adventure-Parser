using SporeApi.Creations;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SporeAdventureParserWPF
{
    /// <summary>
    /// Логика взаимодействия для DownloadProgressWindow.xaml
    /// </summary>
    public partial class DownloadProgressWindow : Window
    {
        //private delegate void DownloadHandler(Creation creation);
        //private delegate void DownloadAdventureHandler(Adventure adventure);

        //private event DownloadHandler DownloadStartEvent;
        //private event DownloadHandler DownloadStopEvent;
        //private event DownloadAdventureHandler DownloadAdventureStartEvent;

        public DownloadProgressWindow()
        {
            InitializeComponent();

            //DownloadStartEvent += Download_Start;
            //DownloadStopEvent += Download_Stop;
            //DownloadAdventureStartEvent += DownloadAdventure_Start;
        }

        // Я пока тупой и не смог реализовать отображение прогресса
        #region DOWNLOAD
        //private void Download_Start(Creation creation) =>
        //    statusText.Text = $"Загрузка творения \"{creation.Name}\"";
        //private void Download_Stop(Creation creation) => progBar.Value++;
        //private void DownloadAdventure_Start(Adventure adventure)
        //{
        //    progBar.IsIndeterminate = false;
        //    if (progBar.Value > 0)
        //        progBar.Maximum += adventure.AssetsCount + 1;
        //    else
        //        progBar.Maximum = adventure.AssetsCount + 1;
        //}

        //private void DownloadCreation(Creation creation)
        //{
        //    //statusText.Text = $"Загрузка творения \"{creation.Name}\"";
        //    //DownloadStartEvent(creation);
        //    CreationDownloader.Download(creation);
        //    //DownloadStopEvent(creation);
        //    //progBar.Value++;
        //}
        public async void DownloadAdventure(params string[] uris)
        {
            Show();
            bool notFailed = true;

            await Task.Run(() =>
            {
                foreach (var uri in uris)
                {
                    notFailed = true;
                    try
                    {
                        Adventure adventure = new Adventure(Creation.Parse(uri));
                        //progBar.IsIndeterminate = false;
                        //progBar.Maximum = adventure.AssetsCount + 1;
                        //DownloadAdventureStartEvent(adventure);
                        //DownloadCreation(adventure);
                        CreationDownloader.Download(adventure);

                        for (int i = 0; i < adventure.AssetsCount; i++)
                            CreationDownloader.Download(adventure.GetCreationFromAssetAt(i));
                            //DownloadCreation(adventure.GetCreationFromAssetAt(i));
                    }
                    catch (Exception ex)
                    {
                        notFailed = false;
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            Close();
            if (notFailed)
            {
                string message = "Приключени";
                if (uris.Length > 1)
                    message += 'я';
                else
                    message += 'е';
                message += " и творения успешно загружены!";
                MessageBox.Show(message, "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
