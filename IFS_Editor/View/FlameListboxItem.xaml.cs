using IFS_Editor.Model;
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

namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for FlameListboxItem.xaml
    /// </summary>
    public partial class FlameListboxItem : ListBoxItem
    {
        private FlameBrowser _fb;
        private Flame _f;

        public FlameListboxItem()
        {//?
            InitializeComponent();
        }

        public FlameListboxItem(FlameBrowser fb, Flame f)
        {
            InitializeComponent();
            _fb = fb;
            _f = f;
            DisplayText.Content = f.name;
        }

        public void UpdateFlame(Flame f)
        {
            _f = f;
            DisplayText.Content = f.name;
            this.Focus();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.RemoveFlame(this);
        }

        private void listBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void listBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Hidden;
        }
    }
}
