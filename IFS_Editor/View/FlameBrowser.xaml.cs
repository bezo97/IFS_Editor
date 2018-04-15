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
        //public NodeMap Map;


        public NodeMap Map
        {
            get { return (NodeMap)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(NodeMap), typeof(FlameBrowser), new PropertyMetadata(null));



        public FlameBrowserVM vm;

        public FlameBrowser()
        {
            vm = new FlameBrowserVM();
            DataContext = vm;
            InitializeComponent();
        }

        public List<FLVM> GetFlames()
        {
            return vm.Flames.ToList();//TODO
        }

        /*public FLVM GetCurrentFlame()
        {
            return vm.SelectedFlame;
        }*/

        public void AddFlame(FLVM f, bool select)
        {
            vm.Flames.Add(f);
            if(select)
                SelectFlame(f);
        }

        public void RemoveFlame(FLVM f)
        {
            int index = vm.Flames.IndexOf(f);
            vm.Flames.Remove(f);

            if (vm.Flames.Count == 0)
                AddFlame(new FLVM(), false/**/);

            //FlameListBox.SelectedIndex = (index < FlameListBox.Items.Count) ? index : 0;
            SelectFlame(vm.Flames[(index < FlameListBox.Items.Count) ? index : FlameListBox.Items.Count-1]);
        }


        public void UpdateAll(FlameBrowserVM vm1)
        {
            //vm = vm1;
            //DataContext = vm;
            vm.FlameCollectionName = vm1.FlameCollectionName;
            vm.Flames.Clear();
            //FlameListBox.Items.Clear();
            foreach (FLVM f in vm1.Flames)
            {
                AddFlame(f,false);
            }
            SelectFlame(vm.Flames[0]);         
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
                vm.Flames[FlameListBox.SelectedIndex] = f;
                SelectFlame(f);
            }
        }

        private void AddFlameButton_Click(object sender, RoutedEventArgs e)
        {
            AddFlame(new FLVM(), true);
        }

        public void SelectFlame(FLVM f)
        {
            vm.SelectedFlame = f;
            Map.Flame = f;
        }
    }
}
