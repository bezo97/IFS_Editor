using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.ViewModel
{
    public class ImagingSettingsViewModel : ObservableObject
    {
        private /*readonly*/ ImagingSettings _s;

        public ImagingSettingsViewModel()
        {

        }

        public ImagingSettingsViewModel(ImagingSettings s)
        {
            _s = s;
        }

        public ImagingSettings _S
        {
            get { return _s; }
        }

        //régen kellett, még jól jöhet
        /*public string BackColor
        {
            get
            {
                //byteok -> hex string
                return "#FF" + ((byte)(_s.Back_colorR * 255)).ToString("X2") + ((byte)(_s.Back_colorG * 255)).ToString("G2") + ((byte)(_s.Back_colorB * 255)).ToString("X2");
            }
            set
            {
                //hex string -> byteok
                _s.Back_colorR = byte.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber)/255.0;
                _s.Back_colorG = byte.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber)/255.0;
                _s.Back_colorB = byte.Parse(value.Substring(7, 2), System.Globalization.NumberStyles.HexNumber)/255.0;
                RaisePropertyChangedEvent("BackColor");
            }
        }*/

        public Color BackColor
        {
            get
            {//byteok -> Color
                return Color.FromArgb(255, (byte)(_s.Back_colorR * 255), (byte)(_s.Back_colorG * 255), (byte)(_s.Back_colorB * 255));
            }
            set
            {//Color -> byteok
                _S.Back_colorR = value.R / 255.0;
                _S.Back_colorG = value.G / 255.0;
                _S.Back_colorB = value.B / 255.0;
                RaisePropertyChangedEvent("BackColor");
            }
        }

        public double Brightness
        {
            get { return _s.Brightness; }
            set { _s.Brightness = value; RaisePropertyChangedEvent("Brightness"); }
        }

        public double Gamma
        {
            get { return _s.Gamma; }
            set { _s.Gamma = value; RaisePropertyChangedEvent("Gamma"); }
        }

        public double G_threshold
        {
            get { return _s.G_threshold; }
            set { _s.G_threshold = value; RaisePropertyChangedEvent("G_threshold"); }
        }


    }
}
