using System;
using System.Collections.Generic;

using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static event PropertyChangedEventHandler StaticPropertyChanged;//erre fel kell iratkozni konstruktorban
        protected static void RaiseStaticPropertyChangedEvent(string propertyName)
        {
            StaticPropertyChanged?.Invoke(/*this*/null, new PropertyChangedEventArgs(propertyName));
        }

    }
}
