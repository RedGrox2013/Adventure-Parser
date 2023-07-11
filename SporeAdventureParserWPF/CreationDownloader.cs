using System;
using System.Net;
using System.IO;
using SporeApi.Creations;

namespace SporeAdventureParserWPF
{
    internal static class CreationDownloader
    {
        private static string _mySporeCreationsPath,
            _adventuresPath, _buildingsPath, _creaturesPath,
            _ufosPath, _vehiclesPath, _directoryName;
        private const string SETTINGS_FILE_NAME = "DownloaderSettings.crtdwn";
        private const string RUS_DIRECTORY_NAME = "Мои творения";

        private static bool _isEnglishFoldersNames;
        public static bool IsEnglishFoldersNames
        {
            get => _isEnglishFoldersNames;
            set
            {
                _isEnglishFoldersNames = value;
                WriteSettings();
            }
        }

        static CreationDownloader()
        {
            if (!File.Exists(SETTINGS_FILE_NAME))
            {
                _mySporeCreationsPath = AutoDetect();
                _isEnglishFoldersNames = _directoryName != RUS_DIRECTORY_NAME;
            }
            else
            {
                var settings = File.ReadAllLines(SETTINGS_FILE_NAME);
                _mySporeCreationsPath = settings[0];
                string[] pathParts = _mySporeCreationsPath.Split('\\');
                _directoryName = pathParts[pathParts.Length - 1];

                if (!bool.TryParse(settings[1], out _isEnglishFoldersNames))
                    _isEnglishFoldersNames = _directoryName != RUS_DIRECTORY_NAME;
            }

            SetPaths();
            WriteSettings();
            CheckFolders();
        }

        public static string AutoDetect()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '\\';
            _directoryName = RUS_DIRECTORY_NAME;
            if (!Directory.Exists(path + _directoryName))
                _directoryName = "My Spore Creations";
            return path + _directoryName;
        }

        private static void WriteSettings()
        {
            string settings = _mySporeCreationsPath + "\n" + IsEnglishFoldersNames;
            File.WriteAllText(SETTINGS_FILE_NAME, settings);
        }

        private static void SetPaths()
        {
            if (!_isEnglishFoldersNames)
            {
                _adventuresPath = _mySporeCreationsPath + "\\Приключения";
                _buildingsPath = _mySporeCreationsPath + "\\Постройки";
                _creaturesPath = _mySporeCreationsPath + "\\Существа";
                _ufosPath = _mySporeCreationsPath + "\\НЛО";
                _vehiclesPath = _mySporeCreationsPath + "\\Техника";
                return;
            }

            _adventuresPath = _mySporeCreationsPath + "\\Adventures";
            _buildingsPath = _mySporeCreationsPath + "\\Buildings";
            _creaturesPath = _mySporeCreationsPath + "\\Creatures";
            _ufosPath = _mySporeCreationsPath + "\\UFOs";
            _vehiclesPath = _mySporeCreationsPath + "\\Vehicles";
        }

        public static string MySporeCreationsPath
        {
            get { return _mySporeCreationsPath; }
            set
            {
                _mySporeCreationsPath = value;
                WriteSettings();
                SetPaths();
                CheckFolders();
            }
        }
        public static string AdventuresPath =>
            _adventuresPath;
        public static string BuildingsPath =>
            _buildingsPath;
        public static string CreaturesPath =>
            _creaturesPath;
        public static string UFOsPath =>
            _ufosPath;
        public static string VehiclesPath =>
            _vehiclesPath;
        public static string DirectoryName =>
            _directoryName;

        private static void CheckFolders()
        {
            if (!Directory.Exists(_mySporeCreationsPath))
                throw new DirectoryNotFoundException("Не найдена папка с творениями (\"" +
                    _mySporeCreationsPath + "\").");

            if (!Directory.Exists(AdventuresPath))
                Directory.CreateDirectory(AdventuresPath);
            if (!Directory.Exists(BuildingsPath))
                Directory.CreateDirectory(BuildingsPath);
            if (!Directory.Exists(CreaturesPath))
                Directory.CreateDirectory(CreaturesPath);
            if (!Directory.Exists(UFOsPath))
                Directory.CreateDirectory(UFOsPath);
            if (!Directory.Exists(VehiclesPath))
                Directory.CreateDirectory(VehiclesPath);
        }

        /// <summary>
        /// Скачивает творение
        /// </summary>
        /// <param name="creation"></param>
        /// <returns>Путь, куда было загружено творение</returns>
        public static string Download(Creation creation)
        {
            string path;
            if (creation.IsAdventure)
                path = AdventuresPath;
            else if (creation.IsBuilding)
                path = BuildingsPath;
            else if (creation.IsCreature)
                path = CreaturesPath;
            else if (creation.IsUFO)
                path = UFOsPath;
            else
                path = VehiclesPath;
            path += $"\\{creation.ID}.png";

            CheckFolders();
            if (!File.Exists(path))
            {
                WebClient client = new WebClient();
                client.DownloadFile(creation.SmallPngUri, path);
                client.Dispose();
            }

            return path;
        }
    }
}
