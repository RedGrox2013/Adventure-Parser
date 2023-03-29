using SporeApi.Creations;
using System;
using System.Windows.Forms;

namespace SporeAdventureParserWPF
{
    public partial class DownloadForm : Form
    {
        public DownloadForm()
        {
            InitializeComponent();
        }

        private void DownloadCreation(Creation creation)
        {
            statusLabel.Text = "Загрузка творения: " + creation.Name;
            statusLabel.Refresh();
            CreationDownloader.Download(creation);
            progBar.Value++;
        }
        public void DownloadAdventure(params string[] uris)
        {
            /*
             * Наитупейшее решение,
             * но ничего лучше я придумать не смог...
             */

            Show();
            Refresh();

            bool notError = false;
            foreach (string uri in uris)
            {
                notError = true;
                statusLabel.Text = "Чтение приключения...";
                progBar.Value = 0;
                statusLabel.Refresh();

                try
                {
                    Adventure adventure = new Adventure(Creation.Parse(uri));
                    progBar.Maximum = 1 + adventure.AssetsCount;
                    DownloadCreation(adventure);

                    for (int i = 0; i < adventure.AssetsCount; i++)
                        DownloadCreation(adventure.GetCreationFromAssetAt(i));
                }
                catch (Exception ex)
                {
                    notError = false;
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //await Task.Run(() =>
            //{
            //    foreach (string uri in uris)
            //    {
            //        notError = true;
            //        statusLabel.Text = "Чтение приключения...";
            //        progBar.Value = 0;
            //        //statusLabel.Refresh();

            //        try
            //        {
            //            Adventure adventure = new Adventure(Creation.Parse(uri));
            //            progBar.Maximum = 1 + adventure.AssetsCount;
            //            DownloadCreation(adventure);

            //            for (int i = 0; i < adventure.AssetsCount; i++)
            //                DownloadCreation(adventure.GetCreationFromAssetAt(i));
            //        }
            //        catch (Exception ex)
            //        {
            //            notError = false;
            //            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //});

            Close();
            if (notError)
            {
                string message = "Приключени";
                if (uris.Length == 1)
                    message += 'е';
                else
                    message += 'я';
                message += " и творения успешно загружены!";
                MessageBox.Show(message, "Готово!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
