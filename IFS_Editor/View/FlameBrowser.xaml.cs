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
using IFS_Editor.ViewModel;

namespace IFS_Editor.View
{
    /// <summary>
    /// Flame kollekcio elemeinek kezelésére View
    /// SelectedIndex valtozasanal a nodemapet automatikusan frissiti
    /// </summary>
    public partial class FlameBrowser : StackPanel
    {
        public NodeMap Map;
        public FlameBrowserVM vm;

        public FlameBrowser()
        {
            vm = new FlameBrowserVM();
            DataContext = vm;
            InitializeComponent();
        }

        public List<FLVM> GetFlames()
        {
            return vm.flames.ToList();//TODO
        }

        public FLVM GetCurrentFlame()
        {
            if (FlameListBox.SelectedIndex < 0)
                FlameListBox.SelectedIndex = 0;
            return vm.flames[FlameListBox.SelectedIndex];
        }

        public void AddFlame(FLVM f, bool select)
        {
            vm.flames.Add(f);
            FlameListboxItem fli = new FlameListboxItem(this,f);
            FlameListBox.Items.Add(fli);
            if (select)
            {
                FlameListBox.SelectedItem = fli;
            }
        }

        public void RemoveFlame(FlameListboxItem fli)
        {
            int index = FlameListBox.Items.IndexOf(fli);
            vm.flames.RemoveAt(index);
            FlameListBox.Items.Remove(fli);

            if (FlameListBox.Items.Count == 0)
                AddFlame(new FLVM(), true);

            FlameListBox.SelectedIndex=(index<FlameListBox.Items.Count)?index:0;
        }

        public void UpdateAll(FlameBrowserVM vm1)
        {
            //vm = vm1;
            //DataContext = vm;
            vm.FlameCollectionName = vm1.FlameCollectionName;
            vm.flames.Clear();
            FlameListBox.Items.Clear();
            foreach (FLVM f in vm1.flames)
            {
                AddFlame(f,false);
            }
            FlameListBox.SelectedIndex = 0;//elsot kivalaszt            
        }

        public void UpdateCurrentFlame(FLVM f)
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
                vm.flames[FlameListBox.SelectedIndex] = f;
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
                Map.Flame = vm.flames[FlameListBox.SelectedIndex];
            }
        }

        private void AddFlameButton_Click(object sender, RoutedEventArgs e)
        {
            AddFlame(new FLVM(), true);
        }
    }
}
