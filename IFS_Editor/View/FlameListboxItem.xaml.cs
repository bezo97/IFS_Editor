using IFS_Editor.ViewModel;
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
    /// Egy flame-t reprezentál a FlameBrowserben
    /// </summary>
    public partial class FlameListboxItem : ListBoxItem
    {
        //private FlameBrowser _fb;
        //private FLVM _f;
        
        public FLVM F
        {
            get { return (FLVM)GetValue(FProperty); }
            set { SetValue(FProperty, value); }
        }

        // Using a DependencyProperty as the backing store for F.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FProperty =
            DependencyProperty.Register("F", typeof(FLVM), typeof(FlameListboxItem), new PropertyMetadata(/*new FLVM()*/null));





        public FlameBrowser Fb
        {
            get { return (FlameBrowser)GetValue(FbProperty); }
            set { SetValue(FbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FbProperty =
            DependencyProperty.Register("Fb", typeof(FlameBrowser), typeof(FlameListboxItem), new PropertyMetadata(null));



        public FlameListboxItem()
        {
            InitializeComponent();
        }

        /*public void UpdateFlame()
        {

        }*/

        /*public FlameListboxItem(FlameBrowser fb, FLVM f)
        {
            InitializeComponent();
            Fb = fb;
            F = f;
            //DisplayText.Content = f.Name;
            DataContext = F;
        }*/

        public void UpdateFlame(FLVM f)
        {
            F = f;
            //DisplayText.Content = f.Name;
            DataContext = F;
            this.Focus();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Fb.RemoveFlame(F);
        }

        private void listBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void listBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Hidden;
        }

        private void ItemSelectedMouse(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            Focus();
            Fb.SelectFlame(F);
        }

    }
}
