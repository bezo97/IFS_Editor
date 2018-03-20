using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class Flame
    {
        string name;
        public CameraSettings cameraSettings = new CameraSettings();
        public RenderSettings renderSettings = new RenderSettings();
        public ImagingSettings imagingSettings = new ImagingSettings();
        List<XForm> xforms = new List<XForm>();
        string palette;

        private List<XForm> XForms { get => xforms; /*set: csak egyenkent, AddXForm()*/ }

        public XForm AddXForm(bool osszekot)
        {
            XForm ujxf = new XForm();
            XForms.Add(ujxf);
            if (osszekot)
                foreach (XForm f in XForms)
                {
                    ujxf.SetConn(new Conn(f, 0.5));
                    f.SetConn(new Conn(ujxf, 0.5));
                }
            return ujxf;
        }

        public List<XForm> GetXForms()
        {
            return XForms;
        }

        

    }
}
