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
        byte back_colorR;
        byte back_colorG;
        byte back_colorB;
        double brightness;
        double gamma;
        double g_threshold;

        public byte Back_colorR { get => back_colorR; set => back_colorR = value; }
        public byte Back_colorG { get => back_colorG; set => back_colorG = value; }
        public byte Back_colorB { get => back_colorB; set => back_colorB = value; }
        public double Brightness { get => brightness; set => brightness = value; }
        public double Gamma { get => gamma; set => gamma = value; }
        public double G_threshold { get => g_threshold; set => g_threshold = value; }

        //wpf color getter

        /*public Color BackColor // helyette: ezt a ViewModel-be
        {
            get { return Color.FromRgb(back_colorR, back_colorG, back_colorB); }
        }*/
    }
}
