using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    public class FlameBrowserVM : ObservableObject
    {
        public ObservableCollection<FLVM> flames;

        public FlameBrowserVM()
        {
            flames = new ObservableCollection<FLVM>();
            FlameCollectionName = "Unnamed Flame Collection";
        }

        public FlameBrowserVM(List<FLVM> fls, string name1)
        {
            flames = new ObservableCollection<FLVM>(fls);
            FlameCollectionName = name1;
        }

        private string fcn = "Unnamed Flame Collection";
        public string FlameCollectionName { get => fcn; set { fcn = value; RaisePropertyChanged("FlameCollectionName"); } }

    }
}
