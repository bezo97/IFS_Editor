using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class XForm
    {
        string name = "linear";
        double color = 0.5;
        double opacity = 1.0;
        double baseWeight = 0.5;
        double[] PreCoefs;
        double[] PostCoefs;

        public List<Conn> Conns = new List<Conn>();

        List<Variation> Vars = new List<Variation>();
    }
}
