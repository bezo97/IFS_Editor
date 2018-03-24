using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string LabelText { get => _xf.name; set { _xf.name = value; RaisePropertyChangedEvent("LabelText"); } }
    }
}
