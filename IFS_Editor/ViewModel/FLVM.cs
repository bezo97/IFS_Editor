using GalaSoft.MvvmLight;
using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    /// <summary>
    /// Flame ViewModel-je
    /// </summary>
    public class FLVM : ObservableObject
    {//flame view model
        private Flame flame;
        List<XFVM> xfs = new List<XFVM>();

        private XFVM sxf;
        public XFVM Selection
        {
            get => sxf; set
            {
                if(sxf!=null)
                    sxf.IsSelected = false;
                sxf = value;
                if (sxf != null)
                    sxf.IsSelected = true;
                RaisePropertyChanged("Selection");
                RaisePropertyChanged("HasSelection");
            }
        }

        public bool HasSelection
        {
            get
            {
                if (Selection == null)
                    return false;
                else
                    return true;
            }
        }

        bool saved = true;
        public bool Saved
        {
            get { return saved; }
            set { saved = value; RaisePropertyChanged("Saved"); }
        }

        /// <summary>
        /// ctor: létrehoz egy üres flame-t
        /// </summary>
        public FLVM()
        {
            flame = new Flame();
        }

        /// <summary>
        /// ctor: meglévő flame-hez kötjük
        /// </summary>
        /// <param name="f"></param>
        public FLVM(Flame f)
        {
            flame = f;
            foreach (XForm xf in f.GetXForms())
            {
                xfs.Add(new XFVM(xf, this));
            }
            //saved = false;
        }

        public XFVM GetXFVMFromXForm(XForm xf)
        {
            foreach (XFVM vm in xfs)
            {
                if (vm.GetXF() == xf)
                    return vm;
            }

            return null;
            //ilyen elv nincs
        }

        public static List<FLVM> FromFlameModels(List<Flame> fs)
        {
            List<FLVM> vms = new List<FLVM>();
            foreach (Flame f in fs)
            {
                vms.Add(new FLVM(f));
            }
            return vms;
        }

        public static List<Flame> ToFlameModels(List<FLVM> vms)
        {
            List<Flame> fs = new List<Flame>();
            foreach (FLVM f in vms)
            {
                fs.Add(f.flame);
            }
            return fs;
        }

        /// <summary>
        /// Új xform hozzáadása a flamehez
        /// </summary>
        /// <param name="osszekot">összekössük-e a meglévő xformokkal</param>
        /// <returns>létrehozott xform viewmodel</returns>
        public XFVM AddXForm(bool osszekot)
        {
            XFVM newxf = new XFVM(flame.AddXForm(osszekot), this);//modelhez hozzaad
            xfs.Add(newxf);//viewmodelhez hozzaad
            Saved = false;
            return newxf;
        }

        public void RemoveXForm(XFVM xf)
        {
            flame.RemoveXForm(xf.GetXF());
            xfs.Remove(xf);
            Saved = false;
        }

        public List<XFVM> GetXForms()
        {
            return xfs;
        }

        //public string EditableText { get => Name; set { Name = value; RaisePropertyChanged("EditableText"); } }
        public string FlameName {
            get => flame.name;
            set { if (flame.name == value) return; flame.name = value; RaisePropertyChanged("FlameName"); Saved = false; } }
        public RenderSettings Render { get => flame.renderSettings; set { flame.renderSettings = value; RaisePropertyChanged("Render"); Saved = false; } }
        public CameraSettings Camera { get => flame.cameraSettings; set { flame.cameraSettings = value; RaisePropertyChanged("Camera"); Saved = false; } }
        public ImagingSettings Imaging { get => flame.imagingSettings; set { flame.imagingSettings = value; RaisePropertyChanged("Imaging"); Saved = false; } }
    }
}
