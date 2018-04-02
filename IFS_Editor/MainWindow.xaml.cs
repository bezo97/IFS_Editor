using IFS_Editor.Serialization;
using IFS_Editor.View;
using IFS_Editor.ViewModel;
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

            flamebrowser_main.AddFlame(new FLVM() { Name = "Unnamed Flame" }, true);
        }

        private void ShowRenderSettingsWindow(object sender, RoutedEventArgs e)
        {
            RenderSettingsWindow rsw = new RenderSettingsWindow(nodemap_main.Flame.Render)
            {
                Owner = this
            };
            if (rsw.ShowDialog()==true)
            {
                nodemap_main.Flame.Render = rsw.GetResult();
            }
        }

        private void ShowImagingSettingsWindow(object sender, RoutedEventArgs e)
        {
            ImagingSettingsWindow isw = new ImagingSettingsWindow(nodemap_main.Flame.Imaging)
            {
                Owner = this
            };
            if (isw.ShowDialog() == true)
            {
                nodemap_main.Flame.Imaging = isw.GetResult();
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
                flamebrowser_main.UpdateAll(FLVM.FromFlameModels(FlameCollectionSerializer.LoadFile(ofd.FileName)), ofd.FileName.Split('\\').Last().Split('.')[0]);
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
                FlameCollectionSerializer.SaveFile(flamebrowser_main.FlameCollectionName, FLVM.ToFlameModels(flamebrowser_main.GetFlames()), sfd.FileName);
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
                FileName = nodemap_main.Flame.Name+" Graph",
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
            flamebrowser_main.UpdateCurrentFlame(new FLVM(FlameSerializer.LoadString(Clipboard.GetText())));
            //try catch
        }

        private void CopyClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(FlameSerializer.SerializeFlame(FLVM.ToFlameModels(new List<FLVM>() { flamebrowser_main.GetCurrentFlame() })[0]).ToString());
            //try catch
        }

        private void NewFlame_Click(object sender, RoutedEventArgs e)
        {
            flamebrowser_main.AddFlame(new FLVM(), true);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //TODO: save before quit?
            Application.Current.Shutdown();
        }

        private void EmptyCollection_Click(object sender, RoutedEventArgs e)
        {
            List<FLVM> fl = new List<FLVM>();
            fl.Add(new FLVM());//1db uj ures flame lesz benne
            flamebrowser_main.UpdateAll(fl, "Unnamed Flame Collection");
        }

        private void DeleteXForm_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.RemoveSelected();
        }

        private void AddLinked_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.AddLinkedXForm();
        }
    }
}
