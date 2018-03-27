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

            nodemap_main.Sidebar = sidebar_main;
            sidebar_main.Map = nodemap_main;
            flamebrowser_main.Map = nodemap_main;

            flamebrowser_main.AddFlame(new Flame() { name = "Unnamed Flame" }, true);
        }

        private void ShowRenderSettingsWindow(object sender, RoutedEventArgs e)
        {
            RenderSettingsWindow rsw = new RenderSettingsWindow(nodemap_main.GetFlame().renderSettings)
            {
                Owner = this
            };
            if (rsw.ShowDialog()==true)
            {
                nodemap_main.GetFlame().renderSettings = rsw.GetResult();
            }
        }

        private void ShowImagingSettingsWindow(object sender, RoutedEventArgs e)
        {
            ImagingSettingsWindow isw = new ImagingSettingsWindow(nodemap_main.GetFlame().imagingSettings)
            {
                Owner = this
            };
            if (isw.ShowDialog() == true)
            {
                nodemap_main.GetFlame().imagingSettings = isw.GetResult();
            }
        }

        private void AddXForm_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.AddXForm();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (ofd.ShowDialog() == true)
            {
                //nodemap_main.SetFlame(FlameSerializer.Load(ofd.FileName));
                flamebrowser_main.Update(FlameSerializer.Load(ofd.FileName));
                //try catch
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = nodemap_main.GetFlame().name,
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (sfd.ShowDialog() == true)
            {
                FlameSerializer.Save(nodemap_main.GetFlame(), sfd.FileName);
                //try catch
            }
        }
    }
}
