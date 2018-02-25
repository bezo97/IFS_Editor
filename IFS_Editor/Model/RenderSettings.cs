using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class RenderSettings
    {
        int sizeX;//resolution
        int sizeY;
        int oversample;
        double filter;
        int quality;

        public int SizeX
        {
            get { return sizeX; }
            set { sizeX = value>0?value:0; }
        }

        public int SizeY
        {
            get { return sizeY; }
            set { sizeY = value > 0 ? value : 0; }
        }

        public int Oversample
        {
            get { return oversample; }
            set { oversample = value > 1 ? value : 1; }
        }

        public double Filter
        {
            get { return filter; }
            set { filter = value > 0 ? value : 0; }
        }

        public int Quality
        {
            get { return quality; }
            set { quality = value > 1 ? value : 1; }
        }
    }
}
