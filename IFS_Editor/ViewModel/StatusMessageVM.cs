using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    /// <summary>
    /// Singleton osztály, üzenetek megjelenítésére az alsó statusbarban (idővel eltűnik)
    /// éppen megnyitott flame collection path-ját is kiírja
    /// </summary>
    public class StatusMessageVM : ObservableObject
    {
        private static readonly StatusMessageVM instance = new StatusMessageVM();
        public static StatusMessageVM Instance { get => instance; }

        System.Windows.Threading.DispatcherTimer timer;

        private StatusMessageVM()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(Hide);
            timer.Interval = new TimeSpan(0, 0, 10);//10mp utan eltunik az uzenet
            
        }

        private void Hide(object sender, EventArgs e)
        {
            message = "";
            RaisePropertyChanged("Text");
            timer.Stop();
        }

        string path="";
        string message="";

        public string Text
        {
            get => path + " | " + message;
        }

        public void Show(string msg)
        {
            message = msg;
            RaisePropertyChanged("Text");
            timer.Start();//10mp ig jelzi ki
        }

        public void SetPath(string p)
        {
            path = p;
            RaisePropertyChanged("Text");
        }

        //TODO: StatusMessage - ikon, hogy az adott üzenet success/error/...

    }
}
