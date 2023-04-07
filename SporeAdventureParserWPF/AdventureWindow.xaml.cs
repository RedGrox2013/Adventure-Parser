using SporeApi.Creations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            advTypeBlock.Text += adventure.ModelType.ToString();
        }
    }
}
