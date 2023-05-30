using SporeApi.Creations;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SporeAdventureParserWPF
{
    /// <summary>
    /// Логика взаимодействия для AdventureWindow.xaml
    /// </summary>
    public partial class AdventureWindow : Window
    {
        public AdventureWindow(Adventure adventure)
        {
            InitializeComponent();

            advNameBlock.Text = adventure.Name;
            advImage.Source = new BitmapImage(new Uri(adventure.LargePngUri));
            authorBlock.Text = adventure.Author;
            createDate.Text = adventure.CreateDate.ToString();
            descriptionBox.Text = adventure.Description;
            if (adventure.TagsCount > 0)
            {
                int lastIndex = adventure.TagsCount - 1;
                for (int i = 0; i < lastIndex; i++)
                    tagsBox.Text += adventure.GetTagAt(i) + ", ";
                tagsBox.Text += adventure.GetTagAt(lastIndex);
            }
            else
                tagsBox.ToolTip = null;
            //advTypeBlock.Text += adventure.ModelType.ToString();
            switch (adventure.ModelType)
            {
                case Modeltype.AdventureAttack:
                    advTypeBlock.Text += "Нападение";
                    break;
                case Modeltype.AdventureCollect:
                    advTypeBlock.Text += "Поиск предметов";
                    break;
                case Modeltype.AdventureDefend:
                    advTypeBlock.Text += "Оборона";
                    break;
                case Modeltype.AdventureExplore:
                    advTypeBlock.Text += "Исследование";
                    break;
                case Modeltype.AdventureSocialize:
                    advTypeBlock.Text += "Общение";
                    break;
                case Modeltype.AdventureStory:
                    advTypeBlock.Text += "История";
                    break;
                case Modeltype.AdventureNoGenre:
                    advTypeBlock.Text += "Нет жанра";
                    break;
                case Modeltype.AdventurePuzzle:
                    advTypeBlock.Text += "Головоломка";
                    break;
                case Modeltype.AdventureQuest:
                    advTypeBlock.Text += "Детектив";
                    break;
                case Modeltype.AdventureTemplate:
                    advTypeBlock.Text += "Шаблон";
                    break;
            }
        }
    }
}
