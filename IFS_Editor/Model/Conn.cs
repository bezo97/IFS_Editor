using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class Conn
    {
        public XForm ConnTo;
        public double WeightTo;

        public Conn (XForm to, double weight)
        {
            ConnTo = to;
            WeightTo = weight;
        }
    }
}
