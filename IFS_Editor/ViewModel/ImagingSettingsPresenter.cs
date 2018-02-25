using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.ViewModel
{
    public class ImagingSettingsPresenter : ObservableObject
    {
        private /*readonly*/ ImagingSettings _s;

        public ImagingSettingsPresenter()
        {

        }

        public ImagingSettingsPresenter(ImagingSettings s)
        {
            _s = s;
        }

        public ImagingSettings _S
        {
            get { return _s; }
        }

        public SolidColorBrush BackColor
        {
            get { return new SolidColorBrush(Color.FromRgb(_s.Back_colorR,_s.Back_colorG,_s.Back_colorB)); }
            set
            {
                _s.Back_colorR = value.Color.R;
                _s.Back_colorG = value.Color.G;
                _s.Back_colorB = value.Color.B;
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
