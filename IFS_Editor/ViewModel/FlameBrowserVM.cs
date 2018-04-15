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
        public ObservableCollection<FLVM> Flames { get; set; }

        private FLVM sf;
        public FLVM SelectedFlame {
            get => sf;
            set { sf = value; RaisePropertyChanged("SelectedFlame"); }
        }

        public FlameBrowserVM()
        {
            Flames = new ObservableCollection<FLVM>();
            Flames.Add(new FLVM());
            SelectedFlame = Flames[0];
            FlameCollectionName = "Unnamed Flame Collection";
        }

        public FlameBrowserVM(List<FLVM> fls, string name1)
        {
            Flames = new ObservableCollection<FLVM>(fls);
            SelectedFlame = Flames[0];
            FlameCollectionName = name1;
        }

        private string fcn = "Unnamed Flame Collection";
        public string FlameCollectionName { get => fcn; set { fcn = value; RaisePropertyChanged("FlameCollectionName"); } }

    }
}
