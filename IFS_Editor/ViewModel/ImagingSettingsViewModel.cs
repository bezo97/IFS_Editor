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

        public string BackColor
        {
            get
            {
                //byteok -> hex
                return "#FF" + _s.Back_colorR.ToString("X") + _s.Back_colorG.ToString("G") + _s.Back_colorB.ToString("X");
            }
            set
            {
                //hex -> byteok
                _s.Back_colorR = byte.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                _s.Back_colorG = byte.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                _s.Back_colorB = byte.Parse(value.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
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
