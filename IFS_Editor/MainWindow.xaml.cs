using IFS_Editor.Model;
using IFS_Editor.Serialization;
using IFS_Editor.View;
using System;
using System.Collections.Generic;
using System.IO;
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
                flamebrowser_main.UpdateAll(FlameCollectionSerializer.LoadFile(ofd.FileName), ofd.FileName);
                //try catch
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = flamebrowser_main.FlameCollectionName,
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (sfd.ShowDialog() == true)
            {
                FlameCollectionSerializer.SaveFile(flamebrowser_main.FlameCollectionName, flamebrowser_main.GetFlames(), sfd.FileName);
                //try catch
            }
        }

        private void SetLayout(object sender, SelectionChangedEventArgs e)
        {
            GraphVizWrapper.Enums.RenderingEngine sel = GraphVizWrapper.Enums.RenderingEngine.Sfdp;
            Enum.TryParse((((ComboBoxItem)LayoutComboBox.SelectedItem).Content?? "Sfdp").ToString(), out sel);
            try
            {
                nodemap_main.GenerateLayout(sel);
            }
            catch
            {
                //TODO: error status
            }
        }

        private void CopyImageToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(nodemap_main.GenerateImage());
            //status
        }

        private void SaveImageToFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = nodemap_main.GetFlame().name+" Graph",
                DefaultExt = ".png",
                Filter = "Image files (.png)|*.png"
            };
            if (sfd.ShowDialog() == true)
            {
                PngBitmapEncoder pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(nodemap_main.GenerateImage()));
                using (Stream fileStream = File.Create(sfd.FileName))
                {
                    pngImage.Save(fileStream);
                }
                //status
            }
        }

        private void PasteClipboard_Click(object sender, RoutedEventArgs e)
        {
            flamebrowser_main.UpdateCurrentFlame(FlameSerializer.LoadString(Clipboard.GetText()));
            //try catch
        }

        private void CopyClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(FlameSerializer.SerializeFlame(nodemap_main.GetFlame()).ToString());
            //try catch
        }

        private void NewFlame_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //TODO: save before quit?
            Application.Current.Shutdown();
        }
    }
}
