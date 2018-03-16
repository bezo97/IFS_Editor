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
        public double baseWeight = 0.5;
        double[] PreCoefs;
        double[] PostCoefs;

        private List<Conn> Conns = new List<Conn>();

        List<Variation> Vars = new List<Variation>();

        public void SetConn(Conn cn)
        {
            foreach (Conn ci in Conns)
            {
                if(ci.ConnTo==cn.ConnTo)
                {
                    if (cn.WeightTo > 0.0)
                        ci.WeightTo = cn.WeightTo;
                    else
                        Conns.Remove(ci);
                    return;
                }
            }
            //meg nem volt benne, akkor hozzaadjuk
            if (cn.WeightTo > 0.0)
                Conns.Add(cn);
        }

        public List<Conn> GetConns()
        {
            return Conns;
        }

    }
}
