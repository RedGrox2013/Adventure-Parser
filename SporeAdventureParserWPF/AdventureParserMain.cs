using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SporeAdventureParserWPF
{
    internal class AdventureParserMain
    {
        /// <summary>
        /// Точка входа
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                RunConsole();
                foreach (var arg in args)
                    Console.WriteLine(arg);

                ShowWindow(GetConsoleWindow(), 0);
                return;
            }

            App app = new App();
            app.Run(new MainWindow());
        }

        //public static void DownloadAdventure(string adventureUri)
        //{
        //    var downloaderWin = new DownloadProgressWindow();
        //    downloaderWin.DownloadAdventure(adventureUri);
        //}

        #region CONSOLE
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void RunConsole()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
                AllocConsole();
            else
                ShowWindow(handle, 5);
        }
        #endregion
    }
}
