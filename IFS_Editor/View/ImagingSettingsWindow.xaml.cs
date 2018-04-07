using IFS_Editor.Model;
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
using System.Windows.Shapes;



namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for ImagingSettingsWindow.xaml
    /// </summary>
    public partial class ImagingSettingsWindow : Window
    {
        ImagingSettingsViewModel isp;

        public ImagingSettingsWindow(ImagingSettings s)
        {
            isp = new ImagingSettingsViewModel(s);
            DataContext = isp;//bindeljuk a viewmodelt a viewhoz
            InitializeComponent();
            //DataContext = isp;//bindeljuk a viewmodelt a viewhoz
        }

        public ImagingSettings GetResult()
        {
            return isp._S;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
