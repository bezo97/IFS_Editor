using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class Flame
    {
        public string name = "Unnamed Flame";
        public string version = "Node Editor";
        public CameraSettings cameraSettings = new CameraSettings();
        public RenderSettings renderSettings = new RenderSettings();
        public ImagingSettings imagingSettings = new ImagingSettings();
        List<XForm> xforms = new List<XForm>();
        public XForm finalxf/* = new XForm()*/;
        public string palette = @"
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
      FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
";//default white palette

        private List<XForm> XForms { get => xforms; /*set: csak egyenkent, AddXForm()*/ }

        public XForm AddXForm(bool osszekot)
        {
            XForm ujxf = new XForm(true);
            XForms.Add(ujxf);
            if (osszekot)
                foreach (XForm f in XForms)
                {
                    ujxf.SetConn(new Conn(f, 1));
                    f.SetConn(new Conn(ujxf, 1));
                }
            return ujxf;
        }

        public void RemoveXForm(XForm xf)
        {
            foreach (XForm f in XForms)
            {//lekapcsoljuk az osszesrol
                f.SetConn(new Conn(xf, 0.0));
            }
            XForms.Remove(xf);
        }

        public void AddXForm(XForm xf)
        {
            XForms.Add(xf);
            //nem kotjuk ossze semmivel!
        }

        public List<XForm> GetXForms()
        {
            return XForms;
        }

        public int XFormCount { get { return XForms.Count; } }

        public XForm DuplicateXForm(XForm a)
        {
            XForm d = new XForm(a);//copy ctr
            XForms.Add(d);
            for(int fi = 0; fi<XForms.Count;fi++)
            {//itt nem lehet foreach mert modositjuk
                for(int ci=0;ci<XForms[fi].GetConns().Count;ci++)
                {
                    if(XForms[fi].GetConns()[ci].ConnTo==a)
                        XForms[fi].SetConn(new Conn(d, 1));
                }
            }
            /*foreach (Conn c in a.GetConns())
            {
                if(c.ConnTo==a)
                {//ha önmaga, akkor az újat is önmagába kötjük
                    d.SetConn(new Conn(d, c.WeightTo));
                }
                else
                 d.SetConn(c);
            }*/
            //eredetit és duplikáltat is összekötjük
            d.SetConn(new Conn(a, 1.0));
            a.SetConn(new Conn(d, 1.0));

            return d;
        }
    }
}
