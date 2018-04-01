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
    /// SelectedIndex valtozasanal a nodemapet automatikusan frissiti
    /// </summary>
    public partial class FlameBrowser : StackPanel
    {
        public NodeMap Map;
        private List<Flame> flames = new List<Flame>();
        private string filepath = "";
        public string FlameCollectionName { set; get; }

        public FlameBrowser()
        {
            InitializeComponent();
        }

        public List<Flame> GetFlames()
        {
            return flames;
        }

        public void AddFlame(Flame f, bool select)
        {
            flames.Add(f);
            FlameListboxItem fli = new FlameListboxItem(this,f);
            /*ListBoxItem li = new ListBoxItem
            {
                Content = f.name//fli
            };*/
            //li.MouseLeftButtonDown += Li_Selected;
            //li.GotFocus += Li_Selected;//->nyillal is lehet valtogatni
            FlameListBox.Items.Add(fli);
            if (select)
            {
                FlameListBox.SelectedItem = fli;
            }
        }

        public void RemoveFlame(FlameListboxItem fli)
        {
            int index = FlameListBox.Items.IndexOf(fli);
            flames.RemoveAt(index);
            FlameListBox.Items.Remove(fli);
            FlameListBox.SelectedIndex=(index<FlameListBox.Items.Count)?index:0;
        }

        public void UpdateAll(List<Flame> LoadedFlames, string path)
        {
            filepath = path;
            FlameCollectionName = path.Split('\\').Last().Split('.')[0];
            FileNameLabel.Content = FlameCollectionName;

            flames.Clear();
            FlameListBox.Items.Clear();
            foreach (Flame f in LoadedFlames)
            {
                AddFlame(f,false);
            }
            FlameListBox.SelectedIndex = 0;//elsot kivalaszt            
        }

        public void UpdateCurrentFlame(Flame f)
        {
            if (FlameListBox.Items.Count == 0)
            {
                AddFlame(f, true);
            }
            else
            {
                if (FlameListBox.SelectedIndex == -1)
                {
                    FlameListBox.SelectedIndex = 0;
                }
                flames[FlameListBox.SelectedIndex] = f;
                ((FlameListboxItem)FlameListBox.SelectedItem).UpdateFlame(f);
                Li_Selected(null,null);//para
            }
        }

        /// <summary>
        /// Nodemapet frissiti az aktualisan kivalasztott flammel.
        /// Automatikusan meghivodik a SelectedIndex valtozasakor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Li_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (FlameListBox.SelectedIndex >= 0)//-1 jelentese listboxnal: nincs kivalasztva semmi
            {
                ((FlameListboxItem)FlameListBox.SelectedItem).Focus();
                Map.SetFlame(flames[FlameListBox.SelectedIndex]);
            }
        }

        private void AddFlameButton_Click(object sender, RoutedEventArgs e)
        {
            AddFlame(new Flame(), true);
        }

        private void RemoveFlameButton_Click(object sender, RoutedEventArgs e)
        {
            //RemoveFlame(FlameListBox.SelectedItem);
        }
    }
}
