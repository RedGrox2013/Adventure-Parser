using System;
using System.Media;
using SporeApi.Creations;

namespace SporeAdventureParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Парсер приключений 228";

            if (args.Length > 0)
            {
                foreach (string arg in args)
                    ReadAdventure(arg);
                return;
            }

            GreetUser();

            do
            {
                Console.Write("Введите ссылку на приключение или его ID: ");
            } while (!ReadAdventure(Console.ReadLine()));

            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }

        public static bool ReadAdventure(string input)
        {
            Adventure adventure;
            Print("Пожалуйста, подождите... Идёт чтение приключения...", ConsoleColor.DarkYellow);
            try
            {
                adventure = new Adventure(Creation.Parse(input));
            }
            catch (Exception ex)
            {
                Print("Ошибка! " + ex.Message, ConsoleColor.Red);
                return false;
            }

            if (!Download(adventure))
                return false;

            for (int i = 0; i < adventure.AssetsCount; i++)
            {
                if (!Download(adventure.GetCreationFromAssetAt(i)))
                    return false;
            }
            Print("\nЗагрузка завершена.\n" +
                "При следующем запуске Spore, " +
                "приключение появится в Спопропедии.\n", ConsoleColor.DarkGreen);

            return true;
        }

        private static bool Download(Creation creation)
        {
            try
            {
                string path = CreationDownloader.Download(creation);
                Print("\nУспешно загружено: ", ConsoleColor.Green, "");
                Print(creation.ToString(), ConsoleColor.DarkYellow, " ");
                Print("(\"", ConsoleColor.Green, "");
                Print(path, ConsoleColor.Yellow, "");
                Print("\")", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Print("Не удалось загрузить:", ConsoleColor.Red, " ");
                Print(creation.ToString(), ConsoleColor.DarkYellow);
                Print(ex.Message +
                    "\nПопробуйте перезапустить программу и повторить попытку.\n" +
                    "Незагрузившийся файл:", ConsoleColor.Red, " ");
                Print(creation.SmallPngUri, ConsoleColor.Blue);
                return false;
            }
            return true;
        }

        private static void Print(object message, ConsoleColor color, string end = "\n")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message + end);
            Console.ForegroundColor = defaultColor;
        }

        private static void GreetUser()
        {
            Console.CursorVisible = false;

            SoundPlayer player;
            try
            {
                player = new SoundPlayer("info.wav");
                player.Play();
            }
            catch
            {
                Print("Ошибка! Боярышник не найден(", ConsoleColor.Red);
                player = null;
            }

            Print("Добро пожаловать в Spore Adventure Parser v1.0.0.0!\n" +
                "Эта программа позволит вам скачать любое приключение с сайта spore.com\n" +
                "Папка, куда будут загружаться творения:", ConsoleColor.Green, " \"");
            Print(CreationDownloader.MySporeCreationsPath, ConsoleColor.Yellow, "");
            Print("\"\nЕсли вы хотете изменить путь, поменяйте его в файле", ConsoleColor.Green, " ");
            Print("MySporeCreationsPath.txt", ConsoleColor.Yellow, " ");
            Print("и перезапустите программу\n" +
                "Если вы хотите вернуть путь по умолчанию, то просто удалите этот файл\n\n" +
                "Мой канал", ConsoleColor.Green, " - ");
            Print("https://www.youtube.com/@RedGrox", ConsoleColor.Blue);
            Print("Второй канал", ConsoleColor.Green, " - ");
            Print("https://www.youtube.com/@RedGrox2013", ConsoleColor.Blue);
            Print("Spore.com", ConsoleColor.Green, " - ");
            Print("http://www.spore.com/view/myspore/RedGrox", ConsoleColor.Blue);
            Print("\nP.S. Если вместо названия творения отображается что-то типа",
                ConsoleColor.Green, " ");
            Print("\"???\"", ConsoleColor.DarkYellow, "");
            Print(", то не пугайтесь, просто творение имеет русские или другие символы " +
                "в названии. В самой игре всё будет отображаться нормально.\n" +
                "Кстати, если вы хотите скачать сразу же все приключения " +
                "Robot Chicken, то запустите файл", ConsoleColor.Green, " \"");
            Print("Robot-Chicken-adventures.bat", ConsoleColor.Yellow, "");
            Print("\".", ConsoleColor.Green);

            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
            Console.CursorVisible = true;
            player?.Stop();
        }
    }
}
