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
    /// Interaction logic for RenderSettingsWindow.xaml
    /// </summary>
    public partial class RenderSettingsWindow : Window
    {
        RenderSettingsPresenter rsp;

        public RenderSettingsWindow(RenderSettings rs)
        {
            rsp = new RenderSettingsPresenter(rs);
            DataContext = rsp;//TODO: bindeljuk a viewmodelt a viewhoz - ezt nem lehetne xamlben megadni???
            InitializeComponent();
        }

        public RenderSettings GetResult()
        {
            return rsp._RS;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RatioComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] ratioValues = ((ComboBoxItem)RatioComboBox.SelectedItem).Content.ToString().Split(':');
            int r1 = Convert.ToInt32((string)ratioValues[0]);
            int r2 = Convert.ToInt32((string)ratioValues[1]);
            if(r1<r2)
                (r1, r2) = (r2, r1); //tuple, uj .NET 4.7 -es feature, most swappelesre
            rsp.SizeX = rsp.SizeY * r1 / r2;
            //pl. 16:9 -> 1920=1080*16/9
            //pl. 9:16 -> 1080=1920*9/16

            //TODO: otlet - configolhato presetek
        }

        private void ResolutionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((ComboBoxItem)ResolutionComboBox.SelectedItem).Content.ToString())
            {
                case "Preview":
                    rsp.SizeY = 340;
                    break;
                case "HDReady":
                    rsp.SizeY = 720;
                    break;
                case "FullHD":
                    rsp.SizeY = 1080;
                    break;
                case "4K":
                    rsp.SizeY = 2160;
                    break;
                case "Printsized":
                    rsp.SizeY = 10000;
                    break;
            }
            RatioComboBox_SelectionChanged(sender, e);//utana a ratiot kiszamoljuk

            //TODO: otlet - configolhato presetek
        }
    }
}
