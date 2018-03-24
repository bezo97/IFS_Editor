using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Editor.Model
{
    public class Variable
    {
        public string Name;
        public double Value;
        public Variable(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
