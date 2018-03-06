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
    /// Interaction logic for XFormSideBar.xaml
    /// </summary>
    public partial class XFormSideBar : Grid
    {
        private XForm xf;
        private NodeMap map;
        public NodeMap Map { set => map = value; get => map; }

        public XFormSideBar()
        {
            InitializeComponent();
            xf = null;
            Visibility = Visibility.Collapsed;
        }

        public XForm Close()
        {
            xf = null;
            Visibility = Visibility.Collapsed;
            Map.SelectedNode = null;
            return xf;
        }
        public void Show(XForm _xf)
        {
            xf = _xf;
            Visibility = Visibility.Visible;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
