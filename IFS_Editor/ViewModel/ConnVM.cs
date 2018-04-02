using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.ViewModel
{
    public class ConnVM : ObservableObject
    {
        private Conn c;
        private XFVM xf;

        public ConnVM(XFVM to, double weight)
        {
            xf = to;
            c = new Conn(to.GetXF(), weight);
        }

        public XFVM ConnTo { get => xf; }
        public double WeightTo { get => c.WeightTo; set { c.WeightTo = value; RaisePropertyChangedEvent("WeightTo"); } }
    }
}
