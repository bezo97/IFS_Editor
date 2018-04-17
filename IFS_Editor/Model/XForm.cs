using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class XForm
    {
        public string name = "linear";
        public double color = 0.0;
        public double opacity = 1.0;
        public double baseWeight = 0.5;
        public double symmetry = 0;//color speed
        public double var_color = 1;//direct color feature in apophysis
        public List<double> PreCoefs = new List<double> { 1, 0, 0, 1, 0, 0 };
        public List<double> PostCoefs = new List<double> { 1, 0, 0, 1, 0, 0 };

        private List<Conn> Conns = new List<Conn>();

        public List<Variation> Variations = new List<Variation>();
        public List<Variable> Variables = new List<Variable>();

        public XForm()
        {

        }

        public XForm(XForm a)
        {
            name = a.name;
            color = a.color;
            opacity = a.opacity;
            baseWeight = a.baseWeight;
            symmetry = a.symmetry;
            var_color = a.var_color;
            PreCoefs = new List<double>(a.PreCoefs);
            PostCoefs = new List<double>(a.PostCoefs);

            Conns = new List<Conn>(a.Conns);

            Variations = new List<Variation>(a.Variations);
            Variables = new List<Variable>(a.Variables);
        }

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

        public void ClearConns()
        {
            Conns.Clear();
        }

        public List<Conn> GetConns()
        {
            return Conns;
        }

    }
}
