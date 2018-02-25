using IFS_Editor.Model;
using IFS_Editor.View;
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

namespace IFS_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowRenderSettingsWindow(object sender, RoutedEventArgs e)
        {
            RenderSettings rs1 = new RenderSettings
            {
                SizeX = 2,
                SizeY = 3,
                Filter = 0.4,
                Oversample = 1,
                Quality = 4000
            };
            RenderSettingsWindow rsw = new RenderSettingsWindow(rs1)
            {
                Owner = this
            };
            if (rsw.ShowDialog()==true)
            {
                rs1 = rsw.GetResult();
            }
        }

        private void ShowImagingSettingsWindow(object sender, RoutedEventArgs e)
        {
            ImagingSettings s1 = new ImagingSettings
            {
                Back_colorR = 255,
                Back_colorG = 0,
                Back_colorB = 0,
                Brightness = 4,
                Gamma = 1,
                G_threshold = 0.025
            };
            ImagingSettingsWindow isw = new ImagingSettingsWindow(s1)
            {
                Owner = this
            };
            if (isw.ShowDialog() == true)
            {
                s1 = isw.GetResult();
            }
        }

        private void AddXForm_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.AddXForm();
        }
    }
}
