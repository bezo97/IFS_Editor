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
        CameraSettings cameraSettings;
        RenderSettings renderSettings;
        ImagingSettings imagingSettings;
        List<XForm> xforms = new List<XForm>();
        string palette;

        public List<XForm> XForms { get => xforms; /*set: csak egyenkent, AddXForm()*/ }

        public XForm AddXForm(bool osszekot)
        {
            XForm ujxf = new XForm();
            if (osszekot)
                foreach (XForm f in XForms)
                {
                    ujxf.Conns.Add(new Conn(f, 0.5));
                    f.Conns.Add(new Conn(ujxf, 0.5));
                }
            XForms.Add(ujxf);
            return ujxf;
        }

        

    }
}
