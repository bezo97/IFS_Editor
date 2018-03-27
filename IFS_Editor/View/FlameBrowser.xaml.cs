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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IFS_Editor.Model;

namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for FlameBrowser.xaml
    /// </summary>
    public partial class FlameBrowser : StackPanel
    {
        public NodeMap Map;
        private List<Flame> flames;
        public FlameBrowser()
        {
            InitializeComponent();
        }

        public void Update(List<Flame> LoadedFlames)
        {
            flames = LoadedFlames;
            FlameListBox.Items.Clear();
            foreach (Flame f in flames)
            {
                ListBoxItem li = new ListBoxItem
                {
                    Content = f.name
                };
                li.Selected += Li_Selected;
                FlameListBox.Items.Add(li);
            }
            FlameListBox.SelectedIndex = 0;//elsot kivalaszt
        }

        private void Li_Selected(object sender, RoutedEventArgs e)
        {
            //kell elmenteni az elozot? sztem nem
            Map.SetFlame(flames[FlameListBox.SelectedIndex]);
        }
    }
}
