using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.Model
{
    public class ImagingSettings
    {
        public ImagingSettings()
        {
            Brightness = 1;
            Gamma = 1;
        }

        public double Back_colorR { get; set; }
        public double Back_colorG { get; set; }
        public double Back_colorB { get; set; }
        public double Brightness { get; set; }
        public double Gamma { get; set; }
        public double G_threshold { get; set; }

        public string Back_ColorString { get { return Back_colorR.ToString() + " " + Back_colorG.ToString() + " " + Back_colorB.ToString(); } }
    }
}
