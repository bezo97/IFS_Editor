using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IFS_Editor.Serialization
{
    public static class XFormSerializer
    {

        public static void SerXForm(XmlWriter w, XForm xf, List<XForm> xfs)
        {
            bool final = false;
            if (xfs == null)
                final = true;

            w.WriteStartElement(final ? "finalxform" : "xform");
            if (!final)
                w.WriteAttributeString("weight", xf.baseWeight.ToString());
            w.WriteAttributeString("color", xf.color.ToString());
            w.WriteAttributeString("symmetry", xf.symmetry.ToString());
            foreach (Variation v in xf.Variations)
            {//variations
                w.WriteAttributeString(v.Name, v.Value.ToString());
                //foreach variable...
            }
            w.WriteAttributeString("coefs", SerializationUtils.GenCoefsString(xf.PreCoefs));
            w.WriteAttributeString("post", SerializationUtils.GenCoefsString(xf.PostCoefs));
            foreach (Variable v in xf.Variables)
            {//variables
                w.WriteAttributeString(v.Name, v.Value.ToString());
            }
            if (!final)
            {
                string xaos = "";
                List<Conn> conns = xf.GetConns();
                /*for (int i=0;i<conns.Count;i++)
                {
                    xaos += conns[i].WeightTo.ToString() + " ";
                }*/
                foreach (XForm xf2 in xfs)
                {
                    double weight = 0.0;//default nincs osszekotve
                    foreach (Conn c in conns)
                    {
                        if (c.ConnTo == xf2)
                        {
                            weight = c.WeightTo;
                            break;
                        }
                    }
                    xaos += weight.ToString() + " ";
                }
                w.WriteAttributeString("chaos", xaos);
                w.WriteAttributeString("opacity", xf.opacity.ToString());
                w.WriteAttributeString("name", xf.name);
            }
            w.WriteAttributeString("var_color", xf.var_color.ToString());//TODO: itt neha elszall???
            w.WriteEndElement();//xform
        }

        public static XForm DeserXForm(XmlReader r)
        {
            XForm xf = new XForm
            {
                name = r["name"] ?? "",//kesobb replace variation nevekkel
                color = double.Parse(r["color"] ?? "0"),
                opacity = double.Parse(r["opacity"] ?? "1"),
                baseWeight = double.Parse(r["weight"] ?? "0.5"),
                symmetry = double.Parse(r["symmetry"] ?? "0"),
                PreCoefs = SerializationUtils.GenCoefsList(r["coefs"]),
                PostCoefs = SerializationUtils.GenCoefsList(r["post"]),
                var_color = double.Parse(r["var_color"] ?? "1")
            };
            SerializationUtils.xaos.Add(r["chaos"]);//null: mindenkivel osszekot

            bool vorv = false;//variation vagy variable olv.
            int attrCnt = r.AttributeCount;
            //r.MoveToFirstAttribute();
            while (r.MoveToNextAttribute())//for (int i=0;i<attrCnt; i++)
            {
                switch (r.Name)
                {
                    case "":
                    case "name":
                    case "color":
                    case "opacity":
                    case "weight":
                    case "symmetry":
                    case "chaos":
                    case "post":
                        continue;//mar kezeltuk
                    case "coefs":
                        vorv = true;//innentol mar csak variable lesz
                        break;
                    default:
                        if (!vorv)
                        {//variation
                            Variation vion = new Variation(r.Name, Double.Parse(r.Value));
                            xf.Variations.Add(vion);
                        }
                        else
                        {//variable
                            Variable vable = new Variable(r.Name, Double.Parse(r.Value));
                            xf.Variables.Add(vable);
                        }
                        break;
                }
            }
            return xf;
        }

    }
}
