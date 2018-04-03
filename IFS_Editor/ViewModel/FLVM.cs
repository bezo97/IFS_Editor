using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    public class FLVM : ObservableObject
    {//flame view model
        private Flame flame;
        List<XFVM> xfs = new List<XFVM>();

        private XFVM sxf;
        public XFVM Selection
        {
            get => sxf; set
            {
                /*if (sxf == value)
                    return;//mar ez van kijelolve, nem kell csinalni semmit
                if (sxf != null)//nem onmaga, akkor eltunik az effekt
                    sxf.EnableEffects(false);
                sxf = value;
                if (sxf != null)
                {
                    sxf.EnableEffects(true);
                    BringNodeToFront(sn);
                    sidebar.Show(sxf.GetXF());
                }
                else
                    sidebar.Close(false);
                updateConnections();*/
                if(sxf!=null)
                    sxf.IsSelected = false;
                sxf = value;
                if (sxf != null)
                    sxf.IsSelected = true;
                RaisePropertyChangedEvent("Selection");
                RaisePropertyChangedEvent("HasSelection");
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

        public FLVM()
        {
            flame = new Flame();
        }

        public FLVM(Flame f)
        {
            flame = f;
            foreach (XForm xf in f.GetXForms())
            {
                xfs.Add(new XFVM(xf, this));
            }
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

        public XFVM AddXForm(bool osszekot)
        {
            XFVM newxf = new XFVM(flame.AddXForm(osszekot), this);//modelhez hozzaad
            xfs.Add(newxf);//viewmodelhez hozzaad
            return newxf;
        }

        public void RemoveXForm(XFVM xf)
        {
            flame.RemoveXForm(xf.GetXF());
            xfs.Remove(xf);
        }

        public List<XFVM> GetXForms()
        {
            return xfs;
        }

        public string Name { get => flame.name; set { flame.name = value; RaisePropertyChangedEvent("Name"); } }
        public RenderSettings Render { get => flame.renderSettings; set { flame.renderSettings = value; RaisePropertyChangedEvent("Render"); } }
        public CameraSettings Camera { get => flame.cameraSettings; set { flame.cameraSettings = value; RaisePropertyChangedEvent("Camera"); } }
        public ImagingSettings Imaging { get => flame.imagingSettings; set { flame.imagingSettings = value; RaisePropertyChangedEvent("Imaging"); } }
    }
}
