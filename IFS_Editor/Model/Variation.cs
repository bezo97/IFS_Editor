using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class Variation
    {
        public string Name;
        public double Value;
        //public List<Variable> Variables = new List<Variable>();//kavaras: kulon tartjuk oket szamon

        public Variation(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
