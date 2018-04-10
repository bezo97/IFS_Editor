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
            EditableText = "Unnamed Flame Collection";
        }

        public FlameBrowserVM(List<FLVM> fls, string name1)
        {
            flames = new ObservableCollection<FLVM>(fls);
            EditableText = name1;
        }

        private string FlameCollectionName = "Unnamed Flame Collection";
        public string EditableText { get => FlameCollectionName; set { FlameCollectionName = value; RaisePropertyChanged("EditableText"); } }

    }
}
