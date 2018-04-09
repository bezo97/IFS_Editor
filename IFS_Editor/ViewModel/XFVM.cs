using GalaSoft.MvvmLight;
using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.ViewModel
{
    /// <summary>
    /// XForm ViewModelje
    /// </summary>
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
                RaiseStaticPropertyChanged("WeightedSize");
                RaiseStaticPropertyChanged("FontSize");
            }
        }

        private static double bs = 100;
        public static double BaseSize {
            get { return bs; }
            set
            {
                bs = value;
                RaiseStaticPropertyChanged("BaseSize");
                RaiseStaticPropertyChanged("WeightedSize");
                RaiseStaticPropertyChanged("FontSize");
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
            StaticPropertyChanged += XFVM_StaticPropertyChanged;//az instance feliratkozik a static eventre
        }

        //static property kiegeszites, akar ObservableObject-be is mehetne
        public static event PropertyChangedEventHandler StaticPropertyChanged;//erre fel kell iratkozni konstruktorban
        protected static void RaiseStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(/*this*/null, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Egy static property változásánál az összes instanceban event lesz
        /// </summary>
        /// <param name="sender">ez null</param>
        /// <param name="e"></param>
        private void XFVM_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {//sender: null
                RaisePropertyChanged(e.PropertyName);//pl WeightedR-nél
        }

        public XForm GetXF()
        {
            //raise event?
            return xf;
        }

        public string Name { get => xf.name; set { xf.name = value; RaisePropertyChanged("Name"); AttachedFlame.Saved = false; } }
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
                RaisePropertyChanged("Opacity");
                //node szin update
                RaisePropertyChanged("OpacityColor");
                AttachedFlame.Saved = false;
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
                RaisePropertyChanged("BaseWeight");
                //node meret update
                if(EnableWeightedSize)
                    RaisePropertyChanged("WeightedSize");
                AttachedFlame.Saved = false;
            }
        }

        public bool IsSelected { get => isselected; set { isselected = value; RaisePropertyChanged("IsSelected"); } }

        public void SetConn(ConnVM c)
        {

            xf.SetConn(new Conn(c.ConnTo.GetXF(), c.WeightTo));
            AttachedFlame.Saved = false;
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
            AttachedFlame.Saved = false;
        }

    }
}
