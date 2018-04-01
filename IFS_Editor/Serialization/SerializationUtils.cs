using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IFS_Editor.Serialization
{
    public static class SerializationUtils
    {
        public static List<string> xaos = new List<string>();
        public static XmlWriterSettings xws = new XmlWriterSettings
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            OmitXmlDeclaration = true,//xml verzio tag nem kell
            Indent = true,
            Encoding = Encoding.Default
        };

        public static Flame Osszerak(Flame f)
        {
            //osszekotesek chaos alapjan + nev adas ha kell
            List<XForm> xfs = f.GetXForms();
            for (int i = 0; i < f.XFormCount; i++)
            {
                string[] tmp = new string[f.XFormCount + 1];
                if (xaos[i] != null)
                    xaos[i]./*Replace('.', ',').*/Split(' ').CopyTo(tmp, 0);
                for (int j = 0; j < f.XFormCount; j++)
                {
                    double weight = 1.0;//default
                    if (tmp[j] != null && tmp[j] != "")
                        weight = Double.Parse(tmp[j]);

                    //if(w!=0.0)//ez nem kell
                    xfs[i].SetConn(new Conn(xfs[j], weight));
                }

                if (xfs[i].name == "" && xfs[i].Variations.Count > 0)//otlet: elnevezzuk, ha nincs neve de van benne variation
                    xfs[i].name = xfs[i].Variations[0].Name;
            }
            return f;
        }

        public static List<double> GenCoefsList(string vals)
        {
            if (vals == null)
                vals = "1 0 0 1 0 0";//default
            List<string> l = vals./*Replace('.',',').*/Split(' ').ToList();
            return l.Select(x => double.Parse(x)).ToList();
        }

        public static string GenCoefsString(List<double> vals)
        {
            string o = "";
            for (int i = 0; i < vals.Count; i++)
            {
                o += vals[i].ToString();
                if (i != vals.Count - 1)
                    o += " ";
            }
            return o;
        }
    }
}
