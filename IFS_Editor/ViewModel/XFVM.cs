using GalaSoft.MvvmLight;
using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string XFName { get => xf.name; set { xf.name = value; RaisePropertyChanged("XFName"); AttachedFlame.Saved = false; } }
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

        public double XFOpacity
        {
            get => xf.opacity;
            set {
                xf.opacity = value;
                RaisePropertyChanged("XFOpacity");
                //node szin update
                RaisePropertyChanged("OpacityColor");
                AttachedFlame.Saved = false;
            }
        }

        public double XFColor
        {
            get => xf.color;
            set
            {
                xf.color = value;
                RaisePropertyChanged("XFColor");
                AttachedFlame.Saved = false;
            }
        }


        public double XFColorSpeed
        {
            get => xf.symmetry;
            set
            {
                xf.symmetry = value;
                RaisePropertyChanged("XFColorSpeed");
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

        public ObservableCollection<Double> PreAffines
        {
            get => new ObservableCollection<Double>(xf.PreCoefs);//ez valid?
            set { xf.PreCoefs = value.ToList(); RaisePropertyChanged("PreAffines"); AttachedFlame.Saved = false; }
        }
        public ObservableCollection<Double> PostAffines
        {
            get => new ObservableCollection<Double>(xf.PostCoefs);//ez valid?
            set { xf.PostCoefs = value.ToList(); RaisePropertyChanged("PostAffines"); AttachedFlame.Saved = false; }
        }
        public void RandomizeAffines()
        {//TODO: ehelyett lehetne egy affine editor ablak, új projekt keretében
            Random r = new Random();
            /*xf.PreCoefs.ForEach(e => e += r.NextDouble()-0.5);
            xf.PostCoefs.ForEach(e => e += r.NextDouble()-0.5);*/ //TODO: ez miért nem jó
            for (int i = 0; i < 6; i++)
            {
                xf.PreCoefs[i] = (r.NextDouble() - 0.5)*3;
                xf.PostCoefs[i] = (r.NextDouble() - 0.5)*3;
            }
            RaisePropertyChanged("PreAffines");
            RaisePropertyChanged("PostAffines");
            AttachedFlame.Saved = false;
        }

        public ObservableCollection<VariationVM> Variations
        {//Lista View-nak OneWay
            get => VariationVM.FromList(xf.Variations);
        }
        public void AddVariation(/*VariationVM vvm*/)
        {
            xf.Variations.Add(new Variation("linear",1.0));
            RaisePropertyChanged("Variations");
            AttachedFlame.Saved = false;
        }

        public ObservableCollection<VariableVM> Variables
        {//Lista View-nak OneWay
            get => VariableVM.FromList(xf.Variables);
        }
        /*public void AddVariable()//VariableVM vvm)
        {
            xf.Variables.Add(new Variable("linear", 1.0));
            RaisePropertyChanged("Variables");
            AttachedFlame.Saved = false;
        }*/

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
