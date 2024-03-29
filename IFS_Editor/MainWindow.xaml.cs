﻿using Fluent;
using IFS_Editor.Serialization;
using IFS_Editor.View;
using IFS_Editor.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            StatusString.DataContext = StatusMessageVM.Instance;
        }

        private void ShowRenderSettingsWindow(object sender, RoutedEventArgs e)
        {
            RenderSettingsWindow rsw = new RenderSettingsWindow(nodemap_main.Flame.Render)
            {
                Owner = this
            };
            if (rsw.ShowDialog() == true)
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
            nodemap_main.AddXForm(-zoombox.Position.X/zoombox.Scale + nodemap_main.Width/ 2 / zoombox.Scale, -zoombox.Position.Y / zoombox.Scale + nodemap_main.Height/ 2 / zoombox.Scale);
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            bool needSave = false;
            foreach (FLVM f in flamebrowser_main.GetFlames())
            {
                if (!f.Saved)
                {
                    needSave = true;
                    break;
                }
            }
            if (needSave)//csak akkor kerdezunk ra, ha van valtozas legalabb egy flameben
            {
                if (MessageBox.Show(this, "This will erase your unsaved work, continue?", "Open Flame Collection", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel)
                    return;
            }

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Open flame collection",
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    string CollectionName = "Unnamed Flame Collection";
                    FlameBrowserVM fbvm = new FlameBrowserVM(FLVM.FromFlameModels(FlameCollectionSerializer.LoadFile(ofd.FileName, out CollectionName)), /*ofd.FileName.Split('\\').Last().Split('.')[0]*/CollectionName);
                    flamebrowser_main.UpdateAll(fbvm);
                    StatusMessageVM.Instance.Show("Flame opened successfully");
                    StatusMessageVM.Instance.SetPath(ofd.FileName);
                }
                catch
                {
                    StatusMessageVM.Instance.Show("Error while opening flame: " + ofd.FileName);
                    StatusMessageVM.Instance.SetPath("");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                Title="Save current flame",
                FileName = flamebrowser_main.vm.FlameCollectionName,
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    FlameCollectionSerializer.SaveFile(flamebrowser_main.vm.FlameCollectionName, FLVM.ToFlameModels(new List<FLVM>() { nodemap_main.Flame }), sfd.FileName);
                    //ezt ide tettem, mert szerializálóba nem lehetett a temporary fájl írások miatt
                    nodemap_main.Flame.Saved = true;
                    StatusMessageVM.Instance.Show("Flame saved successfully");
                }
                catch
                {
                    StatusMessageVM.Instance.Show("Error while saving flame: " + sfd.FileName);
                }
            }
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Save flame collection",
                FileName = flamebrowser_main.vm.FlameCollectionName,
                DefaultExt = ".flame",
                Filter = "Flame files (.flame)|*.flame"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    FlameCollectionSerializer.SaveFile(flamebrowser_main.vm.FlameCollectionName, FLVM.ToFlameModels(flamebrowser_main.GetFlames()), sfd.FileName);
                    foreach (FLVM f in flamebrowser_main.GetFlames())
                    {//ezt ide tettem, mert szerializálóba nem lehetett a temporary fájl írások miatt
                        f.Saved = true;
                    }
                    StatusMessageVM.Instance.Show("Flame collection saved successfully");
                    StatusMessageVM.Instance.SetPath(sfd.FileName);
                }
                catch
                {
                    StatusMessageVM.Instance.Show("Error while saving flame: " + sfd.FileName);
                }
            }
        }

        private void SetLayout(object sender, RoutedEventArgs e)
        {
            GraphVizWrapper.Enums.RenderingEngine sel = GraphVizWrapper.Enums.RenderingEngine.Sfdp;
            string s = ((Fluent.Button)sender).Header.ToString();
            Enum.TryParse( ( s ?? "Sfdp").ToString(), out sel);
            try
            {
                nodemap_main.GenerateLayout(sel);
                StatusMessageVM.Instance.Show("Graph layout generated");
            }
            catch
            {
                StatusMessageVM.Instance.Show("Error while generating layout");
            }
        }

        private void CopyImageToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(nodemap_main.GenerateImage());
            StatusMessageVM.Instance.Show("Image copied to clipboard");
        }

        private void SaveImageToFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = nodemap_main.Flame.FlameName + " Graph",
                DefaultExt = ".png",
                Filter = "Image files (.png)|*.png"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    PngBitmapEncoder pngImage = new PngBitmapEncoder();
                    pngImage.Frames.Add(BitmapFrame.Create(nodemap_main.GenerateImage()));
                    using (Stream fileStream = File.Create(sfd.FileName))
                    {
                        pngImage.Save(fileStream);
                    }
                    StatusMessageVM.Instance.Show("Image saved");
                }
                catch
                {
                    StatusMessageVM.Instance.Show("Error while saving image: " + sfd.FileName);
                }
            }
        }

        private void PasteClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flamebrowser_main.UpdateCurrentFlame(new FLVM(FlameSerializer.LoadString(Clipboard.GetText())));
                StatusMessageVM.Instance.Show("Flame pasted from clipboard");
                StatusMessageVM.Instance.SetPath("");
            }
            catch
            {
                StatusMessageVM.Instance.Show("Error while pasting flame");
            }
        }

        private void CopyClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(FlameSerializer.SerializeFlame(FLVM.ToFlameModels(new List<FLVM>() { flamebrowser_main.vm.SelectedFlame })[0]).ToString());
                StatusMessageVM.Instance.Show("Flame copied to clipboard");
            }
            catch
            {
                StatusMessageVM.Instance.Show("Error while copying flame");
            }
        }

        private void NewFlame_Click(object sender, RoutedEventArgs e)
        {
            flamebrowser_main.AddFlame(new FLVM(), true);
            StatusMessageVM.Instance.Show("New empty flame added");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            bool needSave = false;
            foreach (FLVM f in flamebrowser_main.GetFlames())
            {
                if (!f.Saved)
                {
                    needSave = true;
                    break;
                }
            }

            if (needSave)//csak akkor kerdezunk ra, ha van valtozas legalabb egy flameben
            {
                if (MessageBox.Show(this, "Are you sure you want to quit before saving your work?", "Exit", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.OK)
                    Application.Current.Shutdown();
            }
            else
                Application.Current.Shutdown();
        }

        private void EmptyCollection_Click(object sender, RoutedEventArgs e)
        {
            bool needSave = false;
            foreach (FLVM f in flamebrowser_main.GetFlames())
            {
                if (!f.Saved)
                {
                    needSave = true;
                    break;
                }
            }

            if (needSave)//csak akkor kerdezunk ra, ha van valtozas legalabb egy flameben
            {
                if (MessageBox.Show(this, "This will erase your unsaved work, continue?", "New Flame Collection", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel)
                    return;
            }

            List<FLVM> fl = new List<FLVM>();
            fl.Add(new FLVM());//1db uj ures flame lesz benne
            flamebrowser_main.UpdateAll(new FlameBrowserVM(fl, "Unnamed Flame Collection"));
            StatusMessageVM.Instance.Show("New empty collection generated");
            StatusMessageVM.Instance.SetPath("");

        }

        private void DeleteXForm_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.RemoveSelected();
        }

        private void AddLinked_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.AddLinkedXForm();
        }

        private void Duplicate_Click(object sender, RoutedEventArgs e)
        {
            nodemap_main.DuplicateXForm();
        }

        public bool WeightedRs
        {//toggle toolbar gomb kezeli
            get { return XFVM.EnableWeightedSize; }
            set
            { XFVM.EnableWeightedSize = value; nodemap_main.updateConnections(); }
        }

        private void SetNodeSize(object sender, RoutedEventArgs e)
        {
            string s = ((Fluent.Button)sender).Header.ToString();

            switch (s)
            {
                case "Tiny":
                    XFVM.BaseSize = 25;
                    break;
                case "Small":
                    XFVM.BaseSize = 50;
                    break;
                case "Medium":
                    XFVM.BaseSize = 100;
                    break;
                case "Large":
                    XFVM.BaseSize = 200;
                    break;
            }
            nodemap_main.updateConnections();
        }

        Process procApo;
        private void LaunchApo(object sender, RoutedEventArgs e)
        {
            LaunchProc(ref procApo, @"D:\prog\Apo7X16\Apophysis7X.exe");
            StatusMessageVM.Instance.Show("Flame loaded to Apophysis7X");
        }

        Process procCha;
        private void LaunchCha(object sender, RoutedEventArgs e)
        {
            LaunchProc(ref procCha, @"D:\prog\chaotica_x64_v1.5.8\chaotica.exe");
            StatusMessageVM.Instance.Show("Flame loaded to Chaotica");
        }

        Process procFra;
        private void LaunchFra(object sender, RoutedEventArgs e)
        {
            LaunchProc(ref procFra, @"D:\prog\Apo7X16\Apophysis7X.exe");
            StatusMessageVM.Instance.Show("Flame loaded to Fractorium");
        }

        Process procJwf;
        private void LaunchJwf(object sender, RoutedEventArgs e)
        {
            LaunchProc(ref procJwf, @"D:\prog\Apo7X16\Apophysis7X.exe");
            StatusMessageVM.Instance.Show("Flame loaded to JWildfire");
        }

        private void LaunchProc(ref Process proc, string exepath)
        {
            /*if (proc!=null)
            {//mar fut a process -> beillesztjuk a flamet
                if (!proc.HasExited)
                {
                    //proc.Handle
                    //win api: setforeground és sendkeys:
                    //ctrl+v vel atadas
                    return;
                }
            }*///elvetve
            

            //else: nem fut a process -> elinditjuk tmp fajllal
            Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"Node Editor\");
            string path = System.IO.Path.GetTempPath() + @"Node Editor\" + "tmp.flame";//Guid.NewGuid().ToString()
            FlameCollectionSerializer.SaveFile(flamebrowser_main.vm.FlameCollectionName, FLVM.ToFlameModels(flamebrowser_main.GetFlames()), path);
            proc = new Process();
            proc.StartInfo.FileName = exepath;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetTempPath() + @"Node Editor\";
            proc.StartInfo.Arguments = "tmp.flame";
            //proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.RedirectStandardInput = true;
            proc.Start();
            
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent("Orange"), ThemeManager.GetAppTheme("BaseDark"));

            //ribbonon kivüli dolgok
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));

            //ribbonon kivüli dolgok
        }

        private void Edit_tab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Edit_Contextual.Visibility==Visibility.Visible)
                Edit_tab.IsSelected = true;
        }

        private void Randomize(object sender, RoutedEventArgs e)
        {
            nodemap_main.Flame.Selection.RandomizeAffines();
        }

        private void OpenPastebin(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.pastebin.com");
        }

        private void OpenFF(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.fractalforums.org");
        }

        private void OpenGallery(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.deviantart.com/digitalart/fractals/");
        }

        private void OpenDaTuts(object sender, RoutedEventArgs e)
        {
            Process.Start("https://fractal-resources.deviantart.com/gallery/");
        }

        private void OpenDiscord(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discordapp.com/invite/zEbYqtp");
        }

        private void FinalEdit(object sender, RoutedEventArgs e)
        {
            nodemap_main.Flame.Selection = nodemap_main.Flame.GetFinalXForm();
        }
    }
}
