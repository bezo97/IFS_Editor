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
    /// Interaction logic for XFormSideBar.xaml
    /// </summary>
    public partial class XFormSideBar : Grid
    {
        private XFVM xf;

        public NodeMap Map
        {
            get { return (NodeMap)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(NodeMap), typeof(XFormSideBar), new PropertyMetadata(null));



        public XFormSideBar()
        {
            InitializeComponent();
            xf = null;
            Visibility = Visibility.Collapsed;
        }

        public XFVM Close(bool deselectNode)
        {
            xf = null;
            Visibility = Visibility.Collapsed;
            if(deselectNode)
                Map.SetSelection(null);
            return xf;
        }
        public void Show(XFVM _xf)
        {
            xf = _xf;
            Visibility = Visibility.Visible;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }
    }
}
