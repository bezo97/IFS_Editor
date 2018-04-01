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
        double back_colorR;
        double back_colorG;
        double back_colorB;
        double brightness;
        double gamma;
        double g_threshold;

        public double Back_colorR { get => back_colorR; set => back_colorR = value; }
        public double Back_colorG { get => back_colorG; set => back_colorG = value; }
        public double Back_colorB { get => back_colorB; set => back_colorB = value; }
        public double Brightness { get => brightness; set => brightness = value; }
        public double Gamma { get => gamma; set => gamma = value; }
        public double G_threshold { get => g_threshold; set => g_threshold = value; }

        public string Back_ColorString { get { return back_colorR.ToString() + " " + back_colorG.ToString() + " " + back_colorB.ToString(); } }
    }
}
