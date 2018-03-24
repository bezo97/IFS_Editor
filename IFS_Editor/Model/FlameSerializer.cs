using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IFS_Editor.Model
{
    public class FlameSerializer
    {
        public static Flame Load(string path)
        {
            Flame f = new Flame();
            List<string> xaos = new List<string>();
            using (XmlReader r = XmlReader.Create(path))
            {
                while (r.Read())
                {
                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch(r.Name)
                            {
                                case "flame":
                                    break;
                                case "xform":
                                    XForm xf = new XForm
                                    {
                                        name = r["name"] ?? "",//kesobb replace variation nevekkel
                                        color = Double.Parse(r["color"].Replace('.', ',')),
                                        opacity = Double.Parse(r["opacity"].Replace('.', ',')),
                                        baseWeight = Double.Parse(r["weight"].Replace('.', ',')),
                                        symmetry = Double.Parse((r["symmetry"] ?? "0").Replace('.', ',')),
                                        PreCoefs = GenCoefs(r["coefs"]),
                                        PostCoefs = GenCoefs(r["post"])
                                    };
                                    xaos.Add(r["chaos"]);//null: mindenkivel osszekot

                                    //TODO: variable es variation read: r[i]
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
                                            case "post":
                                                continue;//mar kezeltuk
                                            case "coefs":
                                                vorv = true;//innentol mar csak variable lesz
                                                break;
                                            default:
                                                if(!vorv)
                                                {//variation
                                                    Variation vion = new Variation(r.Name, Double.Parse(r.Value.Replace('.',',')));
                                                    xf.Variations.Add(vion);
                                                }
                                                else
                                                {//variable
                                                    Variable vable = new Variable(r.Name, Double.Parse(r.Value.Replace('.', ',')));
                                                    xf.Variables.Add(vable);
                                                }
                                                break;
                                        }
                                    }

                                    f.AddXForm(xf);
                                    break;
                                case "palette":
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(r.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + r.Name);
                            Console.WriteLine(">");
                            break;
                        default://debug
                            Console.WriteLine(r.NodeType);
                            break;
                    }
                }
            }

            //osszekotesek chaos alapjan + nev adas ha ekll
            List<XForm> xfs = f.GetXForms();
            for (int i = 0; i < f.XFormCount; i++)
            {
                Double[] weights = null;
                if (xaos[i]!=null)//null:mindenkivel osszekot
                    weights = Array.ConvertAll(xaos[i].Replace('.', ',').Split(' '), Double.Parse);
                for (int j = 0; j < f.XFormCount; j++)
                {
                    double w = (weights != null) ? weights[j] : 0.5;
                    //if(w!=0.0)//ez nem kell
                        xfs[i].SetConn(new Conn(xfs[j], w));
                }

                if (xfs[i].name == "")//otlet: elnevezzuk, ha nincs
                    xfs[i].name = xfs[i].Variations[0].Name;//elso variation neve
            }



            return f;
        }

        public static void Save(Flame f, string path)
        {

        }

        private static List<double> GenCoefs(string vals)
        {
            if (vals == null)
                vals = "1 0 0 1 0 0";//default
            List<string> l = vals.Replace('.',',').Split(' ').ToList();
            return l.Select(x => double.Parse(x)).ToList();
        }
    }
}
