using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IFS_Editor.ViewModel
{
    public class NodeViewModel : ObservableObject
    {
        private XForm _xf;

        public NodeViewModel()
        {
            _xf = new XForm();
        }

        public NodeViewModel(XForm xf)
        {
            _xf = xf;
        }

        public XForm _XF
        {
            get { return _xf; }
        }

        public string Name { get => _xf.name; set { _xf.name = value; RaisePropertyChangedEvent("Name"); } }
        public Color Opacity {
            get {
                byte o = (byte)(100 + _xf.opacity*255*0.6);
                return Color.FromRgb(o,o,o);//grayscale
            }
            /*set {
                //ez elhagyhato?
                //
                RaisePropertyChangedEvent("Opacity");
            }*/
        }
    }
}
