using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.ViewModel
{
    public class XFVM : ObservableObject
    {//xform view model
        private XForm xf;//model
        private FLVM AttachedFlame;
        private bool isselected = false;
        //private List<ConnVM> conns = new List<ConnVM>();

        /*public XFVM()
        {
            xf = new XForm();
        }*/

        public XFVM(XForm _xf, FLVM _To)
        {
            xf = _xf;
            AttachedFlame = _To;
        }

        public XForm GetXF()
        {
            //raise event?
            return xf;
        }

        public string Name { get => xf.name; set { xf.name = value; RaisePropertyChangedEvent("Name"); } }
        public Color OpacityColor {
            get {
                byte o = (byte)(100 + xf.opacity*255*0.6);
                return Color.FromRgb(o,o,o);//grayscale
            }
            /*set {
                //ez elhagyhato?
                //
                RaisePropertyChangedEvent("Opacity");
            }*/
        }

        public double Opacity
        {
            get
            {
                return xf.opacity;
            }
            set {
                xf.opacity = value;
                RaisePropertyChangedEvent("Opacity");
                RaisePropertyChangedEvent("OpacityColor");
            }
        }

        public bool IsSelected { get => isselected; set { isselected = value; RaisePropertyChangedEvent("IsSelected"); } }

        public void SetConn(ConnVM c)
        {

            xf.SetConn(new Conn(c.ConnTo.GetXF(), c.WeightTo));
            //raise event
        }

        public List<ConnVM> GetConns()
        {
            List<ConnVM> conns = new List<ConnVM>();
            foreach (Conn c in xf.GetConns())
            {
                conns.Add(new ConnVM(AttachedFlame.GetXFVMFromXForm(c.ConnTo) ?? this, c.WeightTo));
            }
            return conns;
        }

        public void ClearConns()
        {
            xf.ClearConns();
            //raise event
        }

    }
}
