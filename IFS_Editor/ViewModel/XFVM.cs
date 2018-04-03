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

        private static bool ews = false;
        public static bool EnableWeightedSize {
            get { return ews; }
            set {
                ews = value;
                //RaisePropertyChangedEvent("Radius");
                RaiseStaticPropertyChangedEvent("WeightedSize");
                RaiseStaticPropertyChangedEvent("FontSize");
            }
        }

        private static double bs = 100;
        public static double BaseSize {
            get { return bs; }
            set
            {
                bs = value;
                RaiseStaticPropertyChangedEvent("BaseSize");
                RaiseStaticPropertyChangedEvent("WeightedSize");
                RaiseStaticPropertyChangedEvent("FontSize");
            }
        }

        public double WeightedSize
        {
            get
            {
                if (!EnableWeightedSize)
                    return BaseSize;
                return (0.5f + Math.Sqrt(BaseWeight < 10 ? BaseWeight : 10)) * BaseSize;
            }
        }

        public int FontSize
        {
            get
            {
                return (int)(WeightedSize / 5);
            }
        }

        public XFVM(XForm _xf, FLVM _To)
        {
            xf = _xf;
            AttachedFlame = _To;
            StaticPropertyChanged += XFVM_StaticPropertyChanged;
        }

        /// <summary>
        /// Egy static property változásánál az összes instanceban event lesz
        /// </summary>
        /// <param name="sender">ez null</param>
        /// <param name="e"></param>
        private void XFVM_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {//sender: null
                RaisePropertyChangedEvent(e.PropertyName);//pl WeightedR-nél
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
                //node szin update
                RaisePropertyChangedEvent("OpacityColor");
            }
        }

        public double BaseWeight
        {
            get
            {
                return xf.baseWeight;
            }
            set
            {
                xf.baseWeight = value;
                RaisePropertyChangedEvent("BaseWeight");
                //node meret update
                if(EnableWeightedSize)
                    RaisePropertyChangedEvent("WeightedSize");
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
