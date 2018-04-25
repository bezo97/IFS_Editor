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
    public class VariableVM : ObservableObject
    {
        public Variable v;
        public VariableVM(Variable v1)
        {
            v = v1;
        }

        public string VName { get => v.Name; set { v.Name = value; RaisePropertyChanged("VName"); } }
        public Double VValue { get => v.Value; set { v.Value = value; RaisePropertyChanged("VValue"); } }

        public static ObservableCollection<VariableVM> FromList(List<Variable> vs)
        {
            ObservableCollection<VariableVM> vms = new ObservableCollection<VariableVM>();
            foreach (Variable iv in vs)
            {
                vms.Add(new VariableVM(iv));
            }
            return vms;
        }
    }
}
