using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class CameraSettings
    {
        //2d
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Scale { get; set; }
        public double Angle { get; set; }
        public double Rotate { get; set; }
        public double Zoom { get; set; }
        //3d support
        public double Cam_pitch { get; set; }
        public double Cam_yaw { get; set; }
        public double Cam_persp { get; set; }
        public double Cam_zpos { get; set; }
        public double Cam_dof { get; set; }

        public string CenterString { get { return CenterX.ToString() + " " +  CenterY.ToString(); } }

        //polar getterek is
    }
}
