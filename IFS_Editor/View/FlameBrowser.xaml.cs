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
        private List<Flame> flames = new List<Flame>();
        private string filepath = "";

        public FlameBrowser()
        {
            InitializeComponent();
        }

        public void AddFlame(Flame f, bool select)
        {
            flames.Add(f);
            ListBoxItem li = new ListBoxItem
            {
                Content = f.name
            };
            //li.MouseLeftButtonDown += Li_Selected;
            //li.GotFocus += Li_Selected;//->nyillal is lehet valtogatni
            FlameListBox.Items.Add(li);
            if (select)
            {
                FlameListBox.SelectedIndex = FlameListBox.Items.Count - 1;//ujat kivalaszt
                //li.Focus();
            }
        }

        public void Update(List<Flame> LoadedFlames, string path)
        {
            filepath = path;
            FileNameLabel.Content = path.Split('\\').Last();

            flames.Clear();
            FlameListBox.Items.Clear();
            foreach (Flame f in LoadedFlames)
            {
                AddFlame(f,false);
            }
            FlameListBox.SelectedIndex = 0;//elsot kivalaszt
            //((ListBoxItem)FlameListBox.Items.GetItemAt(0)).Focus();
            
        }

        private void Li_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (FlameListBox.SelectedIndex >= 0)//-1 jelentese listboxnal: nincs kivalasztva semmi
                Map.SetFlame(flames[FlameListBox.SelectedIndex]);
        }
    }
}
