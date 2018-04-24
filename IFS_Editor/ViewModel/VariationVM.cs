using GalaSoft.MvvmLight;
using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    public class VariationVM : ObservableObject
    {
        public Variation v;
        public VariationVM(Variation v1)
        {
            v = v1;
        }

        public string VariationName { get => v.Name; set { v.Name = value; RaisePropertyChanged("VariationName"); } }
        public Double VariationValue { get => v.Value; set { v.Value = value; RaisePropertyChanged("VariationValue"); } }

        public static ObservableCollection<VariationVM> FromList(List<Variation> vs)
        {
            ObservableCollection<VariationVM> vms = new ObservableCollection<VariationVM>();
            foreach (Variation iv in vs)
            {
                vms.Add(new VariationVM(iv));
            }
            return vms;
        }

        public static List<Variation> ToList(ObservableCollection<VariationVM> vms)
        {
            List<Variation> vs = new List<Variation>();
            foreach (VariationVM iv in vms)
            {
                vs.Add(iv.v);
            }
            return vs;
        }
    }
}
